CREATE PROCEDURE spCentralizedPrintingOutstandingLetterErrors
	@LetterSeqNum	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*)
	FROM CentralizedPrintingLetterError
	WHERE LetterSeqNum = @LetterSeqNum
		AND Handled IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingOutstandingLetterErrors] TO [db_executor]
    AS [dbo];



