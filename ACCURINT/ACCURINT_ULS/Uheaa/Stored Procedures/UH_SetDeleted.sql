CREATE PROCEDURE [accurint].[UH_SetDeleted]
	@DemosId INT
AS
	UPDATE
		accurint.DemosProcessingQueue_UH
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_NAME()
	WHERE
		DemosId = @DemosId

RETURN 0
