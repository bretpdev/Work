CREATE PROCEDURE [rtnemlinvf].[SetArcAddId]
	@ArcAddId BIGINT,
	@InvalidEmailId INT
AS
	UPDATE
		InvalidEmail
	SET
		ArcAddProcessingId = @ArcAddId
	WHERE
		InvalidEmailId = @InvalidEmailId
RETURN 0