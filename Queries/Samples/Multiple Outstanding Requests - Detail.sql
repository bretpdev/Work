USE BSYS


SELECT	'Script' AS Type
		,B.*
		,A.Requests
FROM	(
			SELECT	A1.Script
					,COUNT(A1.Request) AS Requests
			FROM	dbo.SCKR_DAT_ScriptRequests A1
			WHERE	A1.CurrentStatus NOT IN ('Withdrawn','Complete')
			GROUP BY A1.Script
		) A
		INNER JOIN 
		(		
			SELECT	B1.Request
					,B1.Title
					,B1.Script
					,B1.Priority
					,B1.CurrentStatus
					,CONVERT(VARCHAR,B1.StatusDate,101) AS StatusDate
					,B1.Court
					,CONVERT(VARCHAR,CourtDate,101) AS CourtDate
					,B1.Requester
					,CONVERT(VARCHAR,Requested,101) AS Requested
					,B1.Summary
			FROM	dbo.SCKR_DAT_ScriptRequests B1
			WHERE	B1.CurrentStatus NOT IN ('Withdrawn','Complete')
		) B
			ON A.Script = B.Script
WHERE	A.Requests > 1
ORDER BY B.Script


SELECT	'SAS' AS Type
		,B.*
		,A.Requests
FROM	(
			SELECT	A1.Job
					,COUNT(A1.Request) AS Requests
			FROM	dbo.SCKR_DAT_SASRequests A1
			WHERE	A1.CurrentStatus NOT IN ('Withdrawn','Complete')
			GROUP BY A1.job
		) A
		INNER JOIN 
		(		
			SELECT	B1.Request
					,B1.Title
					,B1.Job
					,B1.Priority
					,B1.CurrentStatus
					,CONVERT(VARCHAR,B1.StatusDate,101) AS StatusDate
					,B1.Court
					,CONVERT(VARCHAR,CourtDate,101) AS CourtDate
					,B1.Requester
					,CONVERT(VARCHAR,Requested,101) AS Requested
					,B1.Summary
			FROM	dbo.SCKR_DAT_SASRequests B1
			WHERE	B1.CurrentStatus NOT IN ('Withdrawn','Complete')
		) B
			ON A.Job = B.Job
WHERE	A.Requests > 1
ORDER BY B.Job


SELECT	'Document' AS Type
		,B.*
		,A.Requests
FROM	(
			SELECT	A1.DocName
					,COUNT(A1.Request) AS Requests
			FROM	dbo.LTDB_DAT_Requests A1
			WHERE	A1.CurrentStatus NOT IN ('Withdrawn','Complete','Post-Implementation Queue','Post-Implementation Review')
			GROUP BY A1.DocName
		) A
		INNER JOIN 
		(		
			SELECT	B1.Request
					,B1.Title
					,B1.DocName
					,B1.Priority
					,B1.CurrentStatus
					,CONVERT(VARCHAR,B1.StatusDate,101) AS StatusDate
					,B1.Court
					,CONVERT(VARCHAR,CourtDate,101) AS CourtDate
					,B1.Requester
					,CONVERT(VARCHAR,Requested,101) AS Requested
					,B1.Requirements
			FROM	dbo.LTDB_DAT_Requests B1
			WHERE	B1.CurrentStatus NOT IN ('Withdrawn','Complete','Post-Implementation Queue','Post-Implementation Review')
		) B
			ON A.DocName = B.DocName
WHERE	A.Requests > 1
ORDER BY B.DocName