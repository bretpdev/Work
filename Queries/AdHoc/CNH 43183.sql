SELECT * FROM CSYS..SYSA_DAT_Users U WHERE U.[Role] = X
		AND U.[Status] = 'Active'

SELECT
	U.FirstName + ' ' + U.LastName AS [Supervisor Name],
	'UNH ' + cast(T.Ticket as varchar(XX)) AS [NH Ticket],
	t.[Subject] as [NH Subject],
	t.Requested as [NH Submission Date]
	

FROM
	NeedHelpCornerStone..DAT_Ticket T
	INNER JOIN NeedHelpCornerStone.[dbo].[DAT_TicketsAssociatedUserID] TA
		ON TA.Ticket = T.Ticket
	INNER JOIN CSYS..SYSA_DAT_Users U
		ON U.SqlUserId = TA.SqlUserId
		AND U.[Role] = X
		AND U.[Status] = 'Active'
WHERE
	Requested >= dateadd(day, -XX, cast(getdate() as date))
	AND 
		( 
			TA.Role = 'Requester'
		)

--UNION ALL

--SELECT
--	U.FirstName + '' + U.LastName AS [Supervisor Name],
--	'CNH ' + cast(T.Ticket as varchar(XX)) AS [NH Ticket],
--	t.[Subject] as [NH Subject],
--	t.Requested as [NH Submission Date]


--FROM
--	NeedHelpUheaa..DAT_Ticket T
--	INNER JOIN [NeedHelpCornerStone].[dbo].[DAT_TicketsAssociatedUserID] TA
--		ON TA.Ticket = T.Ticket
--	INNER JOIN CSYS..SYSA_DAT_Users U
--		ON U.SqlUserId = TA.SqlUserId
--		AND U.[Role] = X
--		AND U.[Status] = 'Active'
--WHERE
--	Requested >= DATEADD(DAY, -XX, GETDATE())
--	AND TA.Role = 'Requester'