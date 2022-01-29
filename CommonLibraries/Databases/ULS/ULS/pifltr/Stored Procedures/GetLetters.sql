CREATE PROCEDURE pifltr.GetLetters
AS
BEGIN
	
	SELECT
		*
	FROM
		pifltr.PifLetter
	WHERE
		(ArcAddedAt IS NULL
		OR PrintedAt IS NULL
		OR ImagedAt IS NULL)
		AND DeletedAt IS NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[pifltr].[GetLetters] TO [db_executor]
    AS [dbo];

