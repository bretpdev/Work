CREATE PROCEDURE [dbo].[GetRequestPriority]
(
	@RequestType VARCHAR(6),
	@RequestId INT
)
AS

SELECT 
	RP.PriorityLevel 
FROM
	RequestPriorities_Ordered RP 
	JOIN
	RequestTypes RT ON RT.RequestTypeId = RP.RequestTypeId
WHERE 
	RT.RequestType = @RequestType
	AND
	RP.RequestId = @RequestId
OPTION (MAXRECURSION 32767)
