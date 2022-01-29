USE BSYS

SELECT	A1.Script
		,COUNT(A1.Request) AS Requests
FROM	dbo.SCKR_DAT_ScriptRequests A1
WHERE	A1.CurrentStatus NOT IN ('Withdrawn','Complete')
GROUP BY A1.Script
HAVING	COUNT(A1.Request) >1

SELECT	A1.Job
		,COUNT(A1.Request) AS Requests
FROM	dbo.SCKR_DAT_SASRequests A1
WHERE	A1.CurrentStatus NOT IN ('Withdrawn','Complete')
GROUP BY A1.job
HAVING	COUNT(A1.Request) >1

SELECT	A1.DocName
		,COUNT(A1.Request) AS Requests
FROM	dbo.LTDB_DAT_Requests A1
WHERE	A1.CurrentStatus NOT IN ('Withdrawn','Complete','Post-Implementation Queue','Post-Implementation Review')
GROUP BY A1.DocName
HAVING	COUNT(A1.Request) >1