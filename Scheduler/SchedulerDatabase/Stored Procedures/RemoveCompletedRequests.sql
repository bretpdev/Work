CREATE PROCEDURE [dbo].[RemoveCompletedRequests]
AS

--remove requests from the priority scheduler that have been moved into a "completed" status
DECLARE @CompletedRequests TABLE(RequestTypeId INT, RequestId INT)
INSERT INTO @CompletedRequests(RequestTypeId, RequestId)
(
	SELECT
		RequestTypeId, Id
	FROM
		SackerCache
	WHERE
		[Status] IN ('Complete','Withdrawn','Publication','Post-Implementation Queue','Promotion','Post-Implementation Review','Test Approval')
)

DECLARE @CompletedRequestIds TABLE(RequestPriorityId INT)
INSERT INTO @CompletedRequestIds(RequestPriorityId)
SELECT 
	RP.RequestPriorityId
FROM
	RequestPriorities RP
	JOIN 
	@CompletedRequests T ON T.RequestTypeId = RP.RequestTypeId AND T.RequestId = RP.RequestId

DECLARE @NewParentId INT
DECLARE @RequestPriorityId INT

WHILE ((SELECT COUNT(*) FROM @CompletedRequestIds) > 0)
BEGIN
	SET @RequestPriorityId = (SELECT TOP 1 RequestPriorityId FROM @CompletedRequestIds)
	SET @NewParentId = (SELECT 
							RequestPriorityId 
						FROM 
							RequestPriorities_Ordered 
						WHERE 
							PriorityLevel = (SELECT MAX(PriorityLevel) FROM RequestPriorities_Ordered))
	--Move the Request to the end of the linked list in preparation for deletion
	EXEC [SetRequestParent] @RequestPriorityId, @NewParentId
	--Delete the request for the scheduler and the Temporary looping table
	DELETE FROM RequestPriorities WHERE RequestPriorityId = @RequestPriorityId
	DELETE FROM @CompletedRequestIds WHERE RequestPriorityID = @RequestPriorityId
END

