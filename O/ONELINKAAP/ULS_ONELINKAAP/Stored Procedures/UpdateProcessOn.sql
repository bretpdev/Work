CREATE PROCEDURE [onelinkaap].[UpdateProcessOn]
	@ArcAddProcessingId BIGINT,
	@Hours INT
AS
	UPDATE 
		AAP
	SET
		ProcessOn = DATEADD(HOUR,@Hours, GETDATE())
	FROM
		ArcAddProcessing AAP
	WHERE 
		ArcAddProcessingId = @ArcAddProcessingId
RETURN 0
