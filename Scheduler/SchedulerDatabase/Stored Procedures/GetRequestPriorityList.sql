CREATE PROCEDURE [dbo].[GetRequestPriorityList]

AS

SELECT 
	T2.RequestPriorityId, 
	T2.ParentId, 
	T2.RequestTypeId, 
	R.RequestType, 
	T2.RequestId, 
	T2.PriorityLevel,
	SR.Name as Name, 
	SR.Court AS CurrentCourt,
	SR.Priority,
	SR.Status,
	SR.DevEstimate,
	SR.TestEstimate
FROM 
	RequestPriorities_Ordered T2 
	JOIN 
	RequestTypes R on R.RequestTypeId = T2.RequestTypeId
	LEFT JOIN
	SackerCache SR ON R.RequestTypeId = SR.RequestTypeId AND SR.Id = T2.RequestId
ORDER BY 
	T2.PriorityLevel
OPTION ( MAXRECURSION 32767)
