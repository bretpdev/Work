CREATE PROCEDURE pifltr.SetImagedAt
	@PifLetterId int
AS
BEGIN
	
	UPDATE
		pifltr.PifLetter
	SET
		ImagedAt = GETDATE()
	WHERE
		PifLetterId = @PifLetterId

END
GO
GRANT EXECUTE
    ON OBJECT::[pifltr].[SetImagedAt] TO [db_executor]
    AS [dbo];

