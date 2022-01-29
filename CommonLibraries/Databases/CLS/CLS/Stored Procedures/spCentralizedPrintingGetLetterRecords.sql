CREATE PROCEDURE [dbo].[spCentralizedPrintingGetLetterRecords]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--check if the application is in recovery (if anything is returned then it is in recovery and a further population should not be selected until the currently selected population is printed)
	IF (SELECT COUNT(*) FROM CentralizedPrintingLetter WHERE PickedUpByCentralizedPrinting IS NOT NULL AND Printed IS NULL) = 0
		BEGIN
			--if not in recovery then reserve population by updating PickedUpByCentralizedPrinting to today
			UPDATE CentralizedPrintingLetter
			SET PickedUpByCentralizedPrinting = GETDATE()
			WHERE PickedUpByCentralizedPrinting IS NULL
		END
	
	SELECT
		SeqNum,
		LetterId,
		AccountNumber,
		BusinessUnitId,
		IsDomestic,
		Requested,
		COALESCE(StateMailBatchSeq, 0) AS StateMailBatchSeq,
		COALESCE(PickedUpByCentralizedPrinting, '1900-1-1') AS PickedUpByCentralizedPrinting,
		COALESCE(Printed, '1900-1-1') AS Printed
	FROM CentralizedPrintingLetter
	WHERE PickedUpByCentralizedPrinting IS NOT NULL
	AND Printed IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingGetLetterRecords] TO [db_executor]
    AS [dbo];



