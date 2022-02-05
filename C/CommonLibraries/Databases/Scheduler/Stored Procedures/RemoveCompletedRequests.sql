CREATE PROCEDURE [dbo].[RemoveCompletedRequests]

AS

--remove requests from the priority scheduler that have been moved into a "completed" status
DECLARE @CompletedRequests TABLE(RequestTypeId INT, RequestId INT)
INSERT INTO @CompletedRequests(RequestTypeId, RequestId)
(
	SELECT 
		RT.RequestTypeId AS RequestTypeId, 
		LT.Request AS RequestId
	FROM [BSYS].[dbo].[LTDB_DAT_Requests] LT 
		INNER JOIN RequestTypes RT ON RT.RequestType = 'Letter'
	WHERE LT.CurrentStatus IN('Complete','Withdrawn','Publication','Post-Implementation Queue','Promotion','Post-Implementation Review','Test Approval')
	
	UNION ALL
	
	SELECT 
		RT.RequestTypeId AS RequestTypeId, 
		SR.Request AS RequestId
	FROM [BSYS].[dbo].[SCKR_DAT_ScriptRequests] SR
		INNER JOIN RequestTypes RT ON RT.RequestType = 'Script'
	WHERE SR.CurrentStatus IN('Complete','Withdrawn','Publication','Post-Implementation Queue','Promotion','Post-Implementation Review','Test Approval')
		
	UNION ALL
	
	SELECT 
		RT.RequestTypeId AS RequestTypeId,
		SASR.Request AS RequestId
	FROM [BSYS].[dbo].[SCKR_DAT_SASRequests] SASR
		INNER JOIN RequestTypes RT ON RT.RequestType = 'SAS'
	WHERE SASR.CurrentStatus IN('Complete','Withdrawn','Publication','Post-Implementation Queue','Promotion','Post-Implementation Review','Test Approval')
)

DECLARE @CompletedRequestIds TABLE(RequestPriorityId INT)
INSERT INTO @CompletedRequestIds(RequestPriorityId)
SELECT 
	RP.RequestPriorityId
FROM [Scheduler].[dbo].RequestPriorities RP
	INNER JOIN @CompletedRequests T ON T.RequestTypeId = RP.RequestTypeId AND T.RequestId = RP.RequestId

DECLARE @NewParentId INT
DECLARE @RequestPriorityId INT

WHILE ((SELECT COUNT(*) FROM @CompletedRequestIds) > 0)
BEGIN
	SET @RequestPriorityId = (SELECT TOP 1 RequestPriorityId FROM @CompletedRequestIds)
	SET @NewParentId = (SELECT RequestPriorityId FROM [Scheduler].dbo.RequestPriorities_Ordered 
						WHERE PriorityLevel = (SELECT MAX(PriorityLevel) FROM [Scheduler].dbo.RequestPriorities_Ordered))
	--Move the Request to the end of the linked list in preparation for deletion
	EXEC [Scheduler].[dbo].[SetRequestParent] @RequestPriorityId, @NewParentId
	--Delete the request for the scheduler and the Temporary looping table
	DELETE FROM [Scheduler].dbo.RequestPriorities WHERE RequestPriorityId = @RequestPriorityId
	DELETE FROM @CompletedRequestIds WHERE RequestPriorityID = @RequestPriorityId
END