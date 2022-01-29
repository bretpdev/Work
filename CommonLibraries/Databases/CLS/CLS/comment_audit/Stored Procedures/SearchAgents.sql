CREATE PROCEDURE [comment_audit].[SearchAgents]
	@FullName nvarchar(MAX),
	@UtId nvarchar(MAX),
	@Active bit = null
AS
	declare @MatchingIds table (AgentId int)
	insert into @MatchingIds (AgentId)
	select distinct a.AgentId
	  from [comment_audit].Agents a
	  left join [comment_audit].AgentUtIds u on a.AgentId = u.AgentId
	 where (@FullName is null or a.FullName like @FullName)
	   and (@UtId is null or u.UtId like @UtId)
	   and (@Active is null or a.Active = @Active)

	select a.AgentId, a.FullName, a.AuditPercentage, a.Active
	  from [comment_audit].Agents a
	  join @MatchingIds id on a.AgentId = id.AgentId

	select a.AgentId, a.UtId
	  from [comment_audit].AgentUtIds a
	  join @MatchingIds id on a.AgentId = id.AgentId
RETURN 0