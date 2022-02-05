CREATE PROCEDURE [comment_audit].[DeleteAgent]
	@AgentId int
AS
	delete
	  from [comment_audit].Agents
	 where AgentId = @AgentId
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[comment_audit].[DeleteAgent] TO [UHEAA\CornerStoneUsers]
    AS [dbo];

