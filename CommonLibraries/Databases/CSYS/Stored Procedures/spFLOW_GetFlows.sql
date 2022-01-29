CREATE PROCEDURE [dbo].[spFLOW_GetFlows]
	@System varchar(30) = ''
AS
BEGIN
	SET NOCOUNT ON;

	IF @System = ''
		SELECT FlowID as FlowName, FlowID as FlowID, [Description] FROM dbo.FLOW_DAT_Flow
    ELSE
		SELECT FlowID as FlowName, FlowID as FlowID, [Description] FROM dbo.FLOW_DAT_Flow
		WHERE [System] = @System
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFLOW_GetFlows] TO [db_executor]
    AS [dbo];


