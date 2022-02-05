CREATE PROCEDURE spCentralizedPrintingLetterError
	@LetterSeqNum	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Check to be sure that an error record for this letter sequence number doesn't already exist.
	IF (SELECT COUNT(*) FROM CentralizedPrintingLetterError WHERE LetterSeqNum = @LetterSeqNum) = 0
		BEGIN
			INSERT INTO CentralizedPrintingLetterError (LetterSeqNum, Detected) VALUES (@LetterSeqNum, GETDATE())
		END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingLetterError] TO [db_executor]
    AS [dbo];



