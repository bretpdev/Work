USE BSYS

SELECT	C.Unit
		,A.Request
		,A.Title
		,A.Script
		,A.Priority
		,A.CurrentStatus
		,CONVERT(VARCHAR,A.StatusDate,101) AS StatusDate
		,A.Court
		,CONVERT(VARCHAR,A.CourtDate,101) AS CourtDate
		,A.Requester
		,CONVERT(VARCHAR,A.Requested,101) AS Requested
		,A.Summary
FROM	dbo.SCKR_DAT_ScriptRequests A
		INNER JOIN dbo.SCKR_DAT_Scripts B
			ON A.Script = B.Script
		INNER JOIN dbo.SCKR_REF_Unit C
			ON A.Script = C.Program
WHERE	A.CurrentStatus NOT IN ('Withdrawn','Complete')
ORDER BY C.Unit, A.Request



SELECT	C.Unit
		,A.Request
		,A.Title
		,A.Job
		,A.Priority
		,A.CurrentStatus
		,CONVERT(VARCHAR,A.StatusDate,101) AS StatusDate
		,A.Court
		,CONVERT(VARCHAR,A.CourtDate,101) AS CourtDate
		,A.Requester
		,CONVERT(VARCHAR,A.Requested,101) AS Requested
		,A.Summary
FROM	dbo.SCKR_DAT_SASRequests A
		INNER JOIN dbo.SCKR_DAT_SAS B
			ON A.Job = B.Job
		INNER JOIN dbo.SCKR_REF_UnitSAS C
			ON A.Job = C.Program
WHERE	A.CurrentStatus NOT IN ('Withdrawn','Complete')
ORDER BY C.Unit, A.Request


SELECT	C.Unit
		,A.Request
		,A.Title
		,A.DocName
		,A.Priority
		,A.CurrentStatus
		,CONVERT(VARCHAR,A.StatusDate,101) AS StatusDate
		,A.Court
		,CONVERT(VARCHAR,A.CourtDate,101) AS CourtDate
		,A.Requester
		,CONVERT(VARCHAR,A.Requested,101) AS Requested
		,A.Requirements
FROM	dbo.LTDB_DAT_Requests A
		INNER JOIN dbo.LTDB_DAT_DocDetail B
			ON A.DocName = B.DocName
		INNER JOIN dbo.LTDB_REF_Unit C
			ON A.DocName = C.DocName
WHERE	A.CurrentStatus NOT IN ('Withdrawn','Complete')
ORDER BY C.Unit, A.Request
