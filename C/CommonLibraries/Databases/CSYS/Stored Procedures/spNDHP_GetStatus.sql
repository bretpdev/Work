CREATE PROCEDURE [dbo].[spNDHP_GetStatus] 
AS
BEGIN

	SET NOCOUNT ON;

	SELECT distinct a.Status
	FROM dbo.FLOW_DAT_FlowStep a
	join dbo.FLOW_DAT_Flow b
	on a.flowid = b.flowid
	where b.system = 'need help general help'

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spNDHP_GetStatus] TO [db_executor]
    AS [dbo];

