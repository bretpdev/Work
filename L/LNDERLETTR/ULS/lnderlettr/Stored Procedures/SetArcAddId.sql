CREATE PROCEDURE [lnderlettr].[SetArcAddId]
	@LettersId INT,
	@ArcAddProcessingId BIGINT
AS
	UPDATE
		[lnderlettr].Letters
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		LettersId = @LettersId