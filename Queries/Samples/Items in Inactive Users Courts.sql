DECLARE @court TABLE( Court Varchar(200))
INSERT INTO @court
SELECT DISTINCT
	I.FirstName + ' ' + I.LastName
FROM
	CSYS..SYSA_DAT_Users I
	LEFT JOIN CSYS..SYSA_DAT_Users A
		ON I.FirstName = A.FirstName
		AND I.LastName = A.LastName
		AND A.Status != 'Inactive'
WHERE
	I.Status = 'Inactive'
	AND A.WindowsUserName IS NULL


SELECT 
	DATEDIFF(dd, CourtDate, GETDATE()) [days_in_court],
	'SR - ' + CAST(SR.Request as VARCHAR(8)) [request],
	SR.[Priority],
	Title,
	Script,
	REPLACE(REPLACE(REPLACE(Court, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	SR.CurrentStatus
FROM
	BSYS.dbo.SCKR_DAT_ScriptRequests SR
WHERE
	SR.CurrentStatus NOT IN ('Complete', 'Withdrawn', 'Draft')
	AND SR.CourtDate IS NOT NULL
	AND REPLACE(REPLACE(REPLACE(COALESCE(Court,''), 'J. ', ''), 'J ', ''), ' iv', '') IN (SELECT DISTINCT Court FROM @court)

UNION ALL

SELECT
	DATEDIFF(dd, SR.CourtDate, GETDATE()) [days_in_court],
	'SASR - ' + CAST(SR.Request as VARCHAR(8)) [request],
	SR.[Priority],
	SR.Title,
	SR.Job,
	REPLACE(REPLACE(REPLACE(Court, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	SR.CurrentStatus
FROM
	BSYS.dbo.SCKR_DAT_SASRequests SR
WHERE
	SR.CurrentStatus NOT IN ('Complete', 'Withdrawn', 'Draft')
	AND SR.CourtDate IS NOT NULL
	AND REPLACE(REPLACE(REPLACE(COALESCE(Court,''), 'J. ', ''), 'J ', ''), ' iv', '') IN (SELECT DISTINCT Court FROM @court)

UNION ALL

SELECT
	DATEDIFF(dd, LT.CourtDate, GETDATE()) [days_in_court],
	'LT - ' + CAST(LT.Request as VARCHAR(8)) [request],
	LT.[Priority],
	LT.Title,
	LT.DocName,
	REPLACE(REPLACE(REPLACE(Court, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	LT.CurrentStatus
FROM
	BSYS.dbo.[LTDB_DAT_Requests] LT
WHERE
	LT.CurrentStatus NOT IN ('Complete', 'Withdrawn', 'Draft')
	AND LT.CourtDate IS NOT NULL
	AND REPLACE(REPLACE(REPLACE(COALESCE(Court,''), 'J. ', ''), 'J ', ''), ' iv', '') IN (SELECT DISTINCT Court FROM @court)

UNION ALL

SELECT
	DATEDIFF(dd, T.CourtDate, GETDATE()) [days_in_court],
	'UNH - ' + CAST(T.Ticket as VARCHAR(10)) [request],
	T.[Priority],
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