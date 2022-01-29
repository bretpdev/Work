CREATE PROCEDURE [comment_audit].[GetAgentsByFullName]
	@FullName nvarchar(max)
AS

	select AgentId, FullName
	  from [comment_audit].Agents
	 where FullName = @FullName

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[comment_audit].[GetAgentsByFullName] TO [UHEAA\CornerStoneUsers]
    AS [dbo];

