CREATE PROCEDURE spCentralizedPrintingMarkLetterAsPrinted
	@LetterSeqNum	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CentralizedPrintingLetter
	SET Printed = GETDATE()
	WHERE SeqNum = @LetterSeqNum
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingMarkLetterAsPrinted] TO [db_executor]
    AS [dbo];



