CREATE PROCEDURE [dbo].[GetRequestPriorityList]

AS

SELECT T2.RequestPriorityId, T2.ParentId, T2.RequestTypeId, R.RequestType, T2.RequestId, T2.PriorityLevel,
CASE WHEN COALESCE(SASR.Title,SR.Title,LT.DocName) = '1900-01-01' THEN NULL ELSE COALESCE(SASR.Title,SR.Title,LT.DocName) END AS Name, 
CASE WHEN COALESCE(SASR.Requested,SR.Requested,LT.Requested) = '1900-01-01' THEN NULL ELSE COALESCE(SASR.Requested,SR.Requested,LT.Requested) END AS Requested, 
CASE WHEN COALESCE(SASR.Court,SR.Court,LT.Court) = '1900-01-01' THEN NULL ELSE COALESCE(SASR.Court,SR.Court,LT.Court) END AS CurrentCourt,
CASE WHEN COALESCE(SASR.Priority,SR.Priority,LT.Priority) = '1900-01-01' THEN NULL ELSE COALESCE(SASR.Priority,SR.Priority,LT.Priority) END AS Priority,
CASE WHEN COALESCE(SASR.CurrentStatus, SR.CurrentStatus, LT.CurrentStatus) = '1900-01-01' THEN NULL ELSE COALESCE(SASR.CurrentStatus, SR.CurrentStatus, LT.CurrentStatus) END AS Status,
CASE WHEN COALESCE(SASR.DevEstimateBegin, SR.DevEstimateBegin, LT.SetupEstimateBegin) = '1900-01-01' THEN NULL ELSE COALESCE(SASR.DevEstimateBegin, SR.DevEstimateBegin, LT.SetupEstimateBegin) END AS DevBegin,
CASE WHEN COALESCE(SASR.DevEstimateEnd, SR.DevEstimateEnd, LT.SetupEstimateEnd) = '1900-01-01' THEN NULL ELSE COALESCE(SASR.DevEstimateEnd, SR.DevEstimateEnd, LT.SetupEstimateEnd) END AS DevEnd,
CASE WHEN COALESCE(SASR.TestEstimateBegin, SR.TestEstimateBegin, LT.TestEstimateBegin) = '1900-01-01' THEN NULL ELSE COALESCE(SASR.TestEstimateBegin, SR.TestEstimateBegin, LT.TestEstimateBegin) END AS TestBegin,
CASE WHEN COALESCE(SASR.TestEstimateEnd, SR.TestEstimateEnd, LT.TestEstimateEnd) = '1900-01-01' THEN NULL ELSE COALESCE(SASR.TestEstimateEnd, SR.TestEstimateEnd, LT.TestEstimateEnd) END AS TestEnd
FROM RequestPriorities_Ordered T2 
INNER JOIN Scheduler.dbo.RequestTypes R on R.RequestTypeId = T2.RequestTypeId
LEFT OUTER JOIN BSYS.dbo.SCKR_DAT_SASRequests SASR ON SASR.Request = T2.RequestId and T2.RequestTypeId = 3
LEFT OUTER JOIN BSYS.dbo.SCKR_DAT_ScriptRequests SR ON SR.Request = T2.RequestId and T2.RequestTypeId = 2
LEFT OUTER JOIN BSYS.dbo.LTDB_DAT_Requests LT ON LT.Request = T2.RequestId and T2.RequestTypeId = 1
ORDER BY 
T2.PriorityLevel
OPTION ( MAXRECURSION 32767)