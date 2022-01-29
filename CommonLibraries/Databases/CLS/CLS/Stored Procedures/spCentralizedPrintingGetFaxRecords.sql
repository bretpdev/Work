CREATE PROCEDURE spCentralizedPrintingGetFaxRecords
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		SeqNum,
		FaxNumber,
		AccountNumber,
		BusinessUnitId,
		LetterId,
		Requested,
		RightFaxHandle,
		COALESCE(Faxed, '1900-1-1') AS Faxed,
		COALESCE(Confirmed, '1900-1-1') AS Confirmed,
		FinalStatus
	FROM CentralizedPrintingFax
	WHERE Faxed IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingGetFaxRecords] TO [db_executor]
    AS [dbo];



