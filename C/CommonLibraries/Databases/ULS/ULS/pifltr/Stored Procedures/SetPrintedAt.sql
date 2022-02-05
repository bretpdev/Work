CREATE PROCEDURE pifltr.SetPrintedAt
	@PifLetterId int
AS
BEGIN
	
	UPDATE
		pifltr.PifLetter
	SET
		PrintedAt = GETDATE()
	WHERE
		PifLetterId = @PifLetterId

END
GO
GRANT EXECUTE
    ON OBJECT::[pifltr].[SetPrintedAt] TO [db_executor]
    AS [dbo];

