CREATE PROCEDURE spCentralizedPrintingFaxError
	@FaxSeqNum	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Check to be sure that an error record for this letter sequence number doesn't already exist.
	IF (SELECT COUNT(*) FROM CentralizedPrintingFaxError WHERE FaxSeqNum = @FaxSeqNum) = 0
		BEGIN
			INSERT INTO CentralizedPrintingFaxError (FaxSeqNum, Detected) VALUES (@FaxSeqNum, GETDATE())
		END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingFaxError] TO [db_executor]
    AS [dbo];



