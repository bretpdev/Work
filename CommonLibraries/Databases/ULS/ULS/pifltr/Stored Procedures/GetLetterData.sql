CREATE PROCEDURE pifltr.GetLetterData
	@PifLetterId int
AS
BEGIN

	SELECT
		*
	FROM
		pifltr.PifLetterData
	WHERE
		PifLetterId = @PifLetterId

END
GO
GRANT EXECUTE
    ON OBJECT::[pifltr].[GetLetterData] TO [db_executor]
    AS [dbo];

