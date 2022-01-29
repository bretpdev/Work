CREATE PROCEDURE [batchesp].[SetTsayDefermentForbearanceAsProcessed]
	@TSAYDefermentForbearanceId INT
AS
	UPDATE
		batchesp.TSAYDefermentForbearances
	SET
		ProcessedAt = GETDATE(),
		ProcessedBy = SYSTEM_USER
	WHERE
		TSAYDefermentForbearanceId = @TSAYDefermentForbearanceId
	
RETURN 0
