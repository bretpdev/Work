CREATE PROCEDURE [payhistlpp].[SetCompletedAt]
	@RunId INT
AS
	UPDATE
		payhistlpp.Run
	SET
		CompletedAt = GETDATE()
	WHERE
		RunId = @RunId