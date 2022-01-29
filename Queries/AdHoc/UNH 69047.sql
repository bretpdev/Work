SELECT * FROM CSYS..SYSA_DAT_Users U WHERE U.[Role] = 5
		AND U.[Status] = 'Active'

SELECT
	U.FirstName + ' ' + U.LastName AS [Supervisor Name],
	'UNH ' + cast(T.Ticket as varchar(20)) AS [NH Ticket],
	t.[Subject] as [NH Subject],
	t.Requested as [NH Submission Date]
	

FROM
	NeedHelpUheaa..DAT_Ticket T
	INNER JOIN [NeedHelpUheaa].[dbo].[DAT_TicketsAssociatedUserID] TA
		ON TA.Ticket = T.Ticket
	INNER JOIN CSYS..SYSA_DAT_Users U
		ON U.SqlUserId = TA.SqlUserId
		AND U.[Role] = 5
		AND U.[Status] = 'Active'
WHERE
	Requested >= '09/01/2020'
	AND 
		( 
			TA.Role = 'Requester'
			OR
			T.History like '%' + U.FirstName + ' ' + U.LastName + '%'
		)

--UNION ALL

--SELECT
--	U.FirstName + '' + U.LastName AS [Supervisor Name],
--	'CNH ' + cast(T.Ticket as varchar(20)) AS [NH Ticket],
--	t.[Subject] as [NH Subject],
--	t.Requested as [NH Submission Date]


--FROM
--	NeedHelpUheaa..DAT_Ticket T
--	INNER JOIN [NeedHelpCornerStone].[dbo].[DAT_TicketsAssociatedUserID] TA
--		ON TA.Ticket = T.Ticket
--	INNER JOIN CSYS..SYSA_DAT_Users U
--		ON U.SqlUserId = TA.SqlUserId
--		AND U.[Role] = 5
--		AND U.[Status] = 'Active'
--WHERE
--	Requested >= DATEADD(DAY, -14, GETDATE())
--	AND TA.Role = 'Requester'