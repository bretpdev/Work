CREATE PROCEDURE [dbo].[CheckForExistingArc]
	@AccountNumber char(10),
	@RecipientId varchar(9),
	@Arc varchar(5),
	@ArcAddDate DateTime

AS
	SELECT
		[RecordId]
	FROM
		[dbo].[ArcAdd]
	WHERE
		AccountNumber = @AccountNumber
		AND RecipientId = @RecipientId
		AND ARC = @Arc
		AND CAST(ArcAddDate AS DATE) = CAST(@ArcAddDate AS DATE)

GRANT EXECUTE ON [dbo].[CheckForExistingArc] TO db_executor

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForExistingArc] TO [db_executor]
    AS [dbo];