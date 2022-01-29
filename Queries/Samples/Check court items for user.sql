DECLARE @court VARCHAR(20)
SET @court = ''
SELECT 
	DATEDIFF(dd, CourtDate, GETDATE()) [days_in_court],
	'SR# ' + CAST(SR.Request as VARCHAR(8)) [request],
	SR.[Priority],
	Title,
	Script,
	REPLACE(REPLACE(REPLACE(Court, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	SR.CurrentStatus
FROM
	BSYS.dbo.SCKR_DAT_ScriptRequests SR
WHERE
	Court IS NOT NULL
	AND
	Court != ''
	AND
	SR.CurrentStatus not in ('Complete', 'Withdrawn', 'Draft')
	AND
	SR.CourtDate is not null
	and
	REPLACE(REPLACE(REPLACE(Court, 'J. ', ''), 'J ', ''), ' iv', '') in (@court)

UNION ALL

SELECT
	DATEDIFF(dd, SR.CourtDate, GETDATE()) [days_in_court],
	'SASR# ' + CAST(SR.Request as VARCHAR(8)) [request],
	SR.[Priority],
	SR.Title,
	SR.Job,
	REPLACE(REPLACE(REPLACE(Court, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	SR.CurrentStatus
FROM
	BSYS.dbo.SCKR_DAT_SASRequests SR
WHERE
	Court IS NOT NULL
	AND
	Court != ''
	AND
	SR.CurrentStatus not in ('Complete', 'Withdrawn', 'Draft')
	AND
	SR.CourtDate is not null
	AND
	REPLACE(REPLACE(REPLACE(Court, 'J. ', ''), 'J ', ''), ' iv', '') in (@court)

UNION ALL

SELECT
	DATEDIFF(dd, LT.CourtDate, GETDATE()) [days_in_court],
	'LT# ' + CAST(LT.Request as VARCHAR(8)) [request],
	LT.[Priority],
	LT.Title,
	LT.DocName,
	REPLACE(REPLACE(REPLACE(Court, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	LT.CurrentStatus
FROM
	BSYS.dbo.[LTDB_DAT_Requests] LT
WHERE
	Court IS NOT NULL
	AND
	Court != ''
	AND
	LT.CurrentStatus not in ('Complete', 'Withdrawn', 'Draft')
	AND
	LT.CourtDate is not null
	AND
	REPLACE(REPLACE(REPLACE(Court, 'J. ', ''), 'J ', ''), ' iv', '') in (@court)

UNION ALL

SELECT
	DATEDIFF(dd, T.CourtDate, GETDATE()) [days_in_court],
	'NH-GH# ' + CAST(T.Ticket as VARCHAR(10)) [request],
	T.[Priority],
	T.[Subject] [Title],
	'N/A',
	REPLACE(REPLACE(REPLACE(SDU.FirstName + ' ' + SDU.LastName, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	T.Status
FROM
	[NeedHelpUHEAA].dbo.DAT_ticket T
	INNER JOIN [NeedHelpUHEAA].dbo.DAT_TicketsAssociatedUserID as TAU on TAU.Ticket = T.Ticket and TAU.[Role] = 'court'
	INNER JOIN [CSYS].dbo.SYSA_DAT_USERS SDU on SDU.SqlUserId = TAU.SqlUserId
WHERE
	T.[Status] not in ('Withdrew', 'Withdrawn', 'Verified', 'Resolved', 'Completed', 'Complete', 'Complete And Verified', 'Draft')
	and
	REPLACE(REPLACE(REPLACE(SDU.FirstName + ' ' + SDU.LastName, 'J. ', ''), 'J ', ''), ' iv', '') in (@court)

UNION ALL

SELECT
	DATEDIFF(dd, T.CourtDate, GETDATE()) [days_in_court],
	'NH-CS# ' + CAST(T.Ticket as VARCHAR(10)) [request],
	T.[Priority],
	T.[Subject] [Title],
	'N/A',
	REPLACE(REPLACE(REPLACE(SDU.FirstName + ' ' + SDU.LastName, 'J. ', ''), 'J ', ''), ' iv', '') [Court],
	T.Status
FROM
	[NeedHelpCornerStone].dbo.DAT_ticket T
	INNER JOIN [NeedHelpCornerStone].dbo.DAT_TicketsAssociatedUserID as TAU on TAU.Ticket = T.Ticket and TAU.[Role] = 'court'
	INNER JOIN [CSYS].dbo.SYSA_DAT_USERS SDU on SDU.SqlUserId = TAU.SqlUserId
WHERE
	T.[Status] not in ('Withdrew', 'Withdrawn', 'Verified', 'Resolved', 'Completed', 'Complete', 'Complete And Verified', 'Draft')
	and
	REPLACE(REPLACE(REPLACE(SDU.FirstName + ' ' + SDU.LastName, 'J. ', ''), 'J ', ''), ' iv', '') in (@court)
ORDER BY
	CurrentStatus, days_in_court DESC