USE NeedHelpUheaa
GO

SELECT
	T.Ticket,
	T.Requested,
	ISNULL(U.WindowsUserName, '') [Windows UserName],
	ISNULL(U.FirstName + ' ' + U.LastName, '') [Name],
	T.[Priority],
	T.[Status]
FROM
	DAT_Ticket T
	LEFT JOIN DAT_TicketsAssociatedUserID TA
		ON T.Ticket = TA.Ticket
		AND TA.[Role] = 'AssignedTo'
	LEFT JOIN CSYS..SYSA_DAT_Users U
		ON TA.SqlUserId = U.SqlUserId
WHERE
	(T.[Status] IN ('Complete','Discussion','In Progress','Review','Systems Support Review') AND T.TicketCode <> 'SASR')
	OR
	(T.[Status] IN ('Complete', 'In Progress') AND T.TicketCode = 'SASR')
ORDER BY
	T.[Status]