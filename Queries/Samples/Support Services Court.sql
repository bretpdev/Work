DECLARE @court TABLE( Court Varchar(200))
INSERT INTO @court
SELECT DISTINCT
	A.FirstName + ' ' + A.LastName
FROM
	CSYS..SYSA_DAT_Users A
WHERE
	A.Status = 'Active'
	AND A.BusinessUnit IN(30,34,35,49,51)


SELECT 
	DATEDIFF(dd, SR.CourtDate, GETDATE()) [days_in_court],
	'SR - ' + CAST(SR.Request as VARCHAR(8)) [request],
	SR.[Priority],
	S.ID AS ScriptId,
	SR.Title,
	SR.Script,
	REPLACE(REPLACE(REPLACE(SR.Court, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	SR.CurrentStatus
FROM
	BSYS.dbo.SCKR_DAT_ScriptRequests SR
	INNER JOIN BSYS.dbo.SCKR_DAT_Scripts S
		ON S.Script = SR.Script
WHERE
	SR.CurrentStatus NOT IN ('Complete', 'Withdrawn', 'Draft')
	AND SR.CourtDate IS NOT NULL
	AND REPLACE(REPLACE(REPLACE(COALESCE(SR.Court,''), 'J. ', ''), 'J ', ''), ' iv', '') IN (SELECT DISTINCT Court FROM @court)

UNION ALL

SELECT
	DATEDIFF(dd, SR.CourtDate, GETDATE()) [days_in_court],
	'SASR - ' + CAST(SR.Request as VARCHAR(8)) [request],
	SR.[Priority],
	S.ID AS SackerId,
	SR.Title,
	SR.Job,
	REPLACE(REPLACE(REPLACE(SR.Court, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	SR.CurrentStatus
FROM
	BSYS.dbo.SCKR_DAT_SASRequests SR
	INNER JOIN BSYS.dbo.SCKR_DAT_SAS S
		ON S.job = SR.Job
WHERE
	SR.CurrentStatus NOT IN ('Complete', 'Withdrawn', 'Draft')
	AND SR.CourtDate IS NOT NULL
	AND REPLACE(REPLACE(REPLACE(COALESCE(SR.Court,''), 'J. ', ''), 'J ', ''), ' iv', '') IN (SELECT DISTINCT Court FROM @court)

UNION ALL

SELECT
	DATEDIFF(dd, LT.CourtDate, GETDATE()) [days_in_court],
	'LT - ' + CAST(LT.Request as VARCHAR(8)) [request],
	LT.[Priority],
	DD.ID AS ScriptId,
	LT.Title,
	LT.DocName,
	REPLACE(REPLACE(REPLACE(LT.Court, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	LT.CurrentStatus
FROM
	BSYS.dbo.[LTDB_DAT_Requests] LT
	INNER JOIN BSYS.dbo.LTDB_DAT_DocDetail DD
		ON DD.DocName = LT.DocName
WHERE
	LT.CurrentStatus NOT IN ('Complete', 'Withdrawn', 'Draft')
	AND LT.CourtDate IS NOT NULL
	AND REPLACE(REPLACE(REPLACE(COALESCE(LT.Court,''), 'J. ', ''), 'J ', ''), ' iv', '') IN (SELECT DISTINCT Court FROM @court)

UNION ALL

SELECT
	DATEDIFF(dd, T.CourtDate, GETDATE()) [days_in_court],
	'UNH - ' + CAST(T.Ticket as VARCHAR(10)) [request],
	T.[Priority],
	'',
	T.[Subject] [Title],
	'N/A',
	REPLACE(REPLACE(REPLACE(SDU.FirstName + ' ' + SDU.LastName, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	T.Status
FROM
	[NeedHelpUHEAA].dbo.DAT_ticket T
	INNER JOIN [NeedHelpUHEAA].dbo.DAT_TicketsAssociatedUserID TAU 
		ON TAU.Ticket = T.Ticket 
		AND TAU.[Role] = 'court'
	INNER JOIN [CSYS].dbo.SYSA_DAT_USERS SDU 
		ON SDU.SqlUserId = TAU.SqlUserId
WHERE
	T.[Status] NOT IN ('Withdrew', 'Withdrawn', 'Verified', 'Resolved', 'Completed', 'Complete', 'Complete And Verified', 'Draft')
	AND REPLACE(REPLACE(REPLACE(SDU.FirstName + ' ' + SDU.LastName, 'J. ', ''), 'J ', ''), ' iv', '') IN (SELECT DISTINCT Court FROM @court)

UNION ALL

SELECT
	DATEDIFF(dd, T.CourtDate, GETDATE()) [days_in_court],
	'CNH - ' + CAST(T.Ticket as VARCHAR(10)) [request],
	T.[Priority],
	'',
	T.[Subject] [Title],
	'N/A',
	REPLACE(REPLACE(REPLACE(SDU.FirstName + ' ' + SDU.LastName, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	T.Status
FROM
	[NeedHelpCornerStone].dbo.DAT_ticket T
	INNER JOIN [NeedHelpCornerStone].dbo.DAT_TicketsAssociatedUserID TAU 
		ON TAU.Ticket = T.Ticket 
		AND TAU.[Role] = 'court'
	INNER JOIN [CSYS].dbo.SYSA_DAT_USERS SDU 
		ON SDU.SqlUserId = TAU.SqlUserId
WHERE
	T.[Status] NOT IN ('Withdrew', 'Withdrawn', 'Verified', 'Resolved', 'Completed', 'Complete', 'Complete And Verified', 'Draft')
	AND REPLACE(REPLACE(REPLACE(SDU.FirstName + ' ' + SDU.LastName, 'J. ', ''), 'J ', ''), ' iv', '') in (SELECT DISTINCT Court FROM @court)
ORDER BY
	Court, CurrentStatus DESC