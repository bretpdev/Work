CREATE PROCEDURE [comment_audit].[GetAgentsByUtId]
	@UtId char(6)
AS

	select a.AgentId, a.FullName
	  from [comment_audit].Agents a
	  join [comment_audit].AgentUtIds u on a.AgentId = u.AgentId
	 where u.UtId = @UtId

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[comment_audit].[GetAgentsByUtId] TO [UHEAA\CornerStoneUsers]
    AS [dbo];

