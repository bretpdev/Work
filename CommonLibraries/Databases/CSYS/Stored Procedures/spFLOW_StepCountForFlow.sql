CREATE PROCEDURE [dbo].[spFLOW_StepCountForFlow] 
	@FlowID									VARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT COUNT(*)	
	FROM dbo.FLOW_DAT_FlowStep
	WHERE FlowID = @FlowID

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_StepCountForFlow] TO [db_executor]
    AS [dbo];


