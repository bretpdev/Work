CREATE PROCEDURE pifltr.SetArcAddedAt
	@PifLetterId int
AS
BEGIN
	
	UPDATE
		pifltr.PifLetter
	SET
		ArcAddedAt = GETDATE()
	WHERE
		PifLetterId = @PifLetterId

END
GO
GRANT EXECUTE
    ON OBJECT::[pifltr].[SetArcAddedAt] TO [db_executor]
    AS [dbo];

