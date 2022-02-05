CREATE PROCEDURE [accurint].[OL_SetDeleted]
	@DemosId INT
AS
	UPDATE
		accurint.DemosProcessingQueue_OL
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_NAME()
	WHERE
		DemosId = @DemosId
		

RETURN 0
