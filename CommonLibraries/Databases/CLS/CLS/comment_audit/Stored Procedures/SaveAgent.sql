CREATE PROCEDURE [comment_audit].[SaveAgent]
	@AgentId int = null,
	@FullName nvarchar(max),
	@Active bit,
	@AuditPercentage decimal(5, 2),
	@UtIds UtId readonly
AS
	--update agents table
	if @AgentId is null 
	begin
		insert into [comment_audit].Agents (FullName, Active, AuditPercentage)
		values (@FullName, @Active, @AuditPercentage)

		set @AgentId = SCOPE_IDENTITY()
	end
	else
	begin
		update [comment_audit].Agents
		   set FullName = @FullName, Active = @Active, AuditPercentage = @AuditPercentage
		 where AgentId = @AgentId
	end

	--sync utids
	delete 
	  from [comment_audit].AgentUtIds
	 where UtId not in (select UtId from @UtIds)
	   and AgentId = @AgentId

	insert 
	  into [comment_audit].AgentUtIds (AgentId, UtId)
	select @AgentId, UtId
	  from @UtIds
	 where UtId not in (select UtId from [comment_audit].AgentUtIds where AgentId = @Agentid)

select @AgentId