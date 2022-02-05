CREATE PROCEDURE [rhbrwinpc].[SetArcAddId]
	@LettersId INT,
	@ArcAddProcessingId INT
AS
	UPDATE
		[rhbrwinpc].Letters
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		LettersId = @LettersId