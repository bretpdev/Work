CREATE PROCEDURE [onelinkaap].[UpdateLN_ATY_SEQ]
	@ArcAddProcessingId BIGINT,
	@LN_ATY_SEQ int
AS
	UPDATE
		ArcAddProcessing
	SET
		LN_ATY_SEQ = @LN_ATY_SEQ
	WHERE
		ArcAddProcessingId = @ArcAddProcessingId
RETURN 0
