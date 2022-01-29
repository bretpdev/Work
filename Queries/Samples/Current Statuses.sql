USE BSYS

SELECT	A.CurrentStatus, COUNT(A.Request) AS Requests
FROM	dbo.SCKR_DAT_ScriptRequests A
GROUP BY A.CurrentStatus
ORDER BY A.CurrentStatus

SELECT	A.CurrentStatus, COUNT(A.Request) AS Requests
FROM	dbo.SCKR_DAT_SASRequests A
GROUP BY A.CurrentStatus
ORDER BY A.CurrentStatus

SELECT	A.CurrentStatus, COUNT(A.Request) AS Requests
FROM	dbo.LTDB_DAT_Requests A
GROUP BY A.CurrentStatus
ORDER BY A.CurrentStatus