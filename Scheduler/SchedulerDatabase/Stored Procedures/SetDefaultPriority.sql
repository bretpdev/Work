CREATE PROCEDURE [dbo].[SetDefaultPriority]
(
	@RequestType VARCHAR(6),
	@RequestId INT
)
AS

DECLARE @Parent INT = (SELECT MAX(RP.RequestPriorityId) from RequestPriorities RP)

INSERT INTO 
	RequestPriorities(ParentId, RequestTypeId, RequestId)
SELECT 
	@Parent, RT.RequestTypeId, @RequestId
FROM 
	RequestTypes RT
WHERE 
	RT.RequestType = @RequestType
