CREATE PROCEDURE [dbo].[SetRequestParent]
	@RequestPriorityId int,
	@NewParentId int = null
AS
SET XACT_ABORT ON
	--gather all possibly affected nodes
	declare @CurrentParentId int
	 select @CurrentParentID = ParentId
	   from RequestPriorities
	  where RequestPriorityId = @RequestPriorityId

	declare @CurrentChildId int
  	 select @CurrentChildID = RequestPriorityId
	   from RequestPriorities
	  where ParentId = @RequestPriorityId
	
	declare @NewChildId int
	 select @NewChildId = RequestPriorityId
	   from RequestPriorities
	  where ParentId = @NewParentID or (ParentId is null and @NewParentId is null)

         if (@NewParentId = @CurrentParentId) return

SET XACT_ABORT ON
BEGIN TRANSACTION

	 alter table RequestPriorities DROP CONSTRAINT [AK_RequestPriorities_PrioritizedAfter]

	--mend the gap that will be introduced by removing the existing node from its position
	update RequestPriorities
	   set ParentId = @CurrentParentId  
	 where RequestPriorityId = @CurrentChildId

	--link the new child to the existing node
	update RequestPriorities
	   set ParentId = @RequestPriorityId
	 where RequestPriorityId = @NewChildId
	
	--link the existing node to the new parent
	update RequestPriorities
	   set ParentId = @NewParentId
	 where RequestPriorityId = @RequestPriorityId   

	 alter table RequestPriorities ADD CONSTRAINT [AK_RequestPriorities_PrioritizedAfter] UNIQUE ([ParentId])

COMMIT TRANSACTION
RETURN 0