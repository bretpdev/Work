USE NeedHelpUheaa
GO

SELECT
	TKT.Ticket AS TicketNumber,
	CAST(TKT.StatusDate AS DATE) AS StatusDate,
	TKT.[Status] AS [Status],
	CAST(TKT.Requested AS DATE) AS RequestedDate,
	TKT.[Priority] AS [Priority]
FROM
	DAT_Ticket TKT
WHERE
	TKT.[Status] in ('Discussion', 'Review', 'Hold', 'Systems Support Review')
ORDER BY
	Ticket ASC