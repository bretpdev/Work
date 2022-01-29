SELECT DISTINCT
	T.Ticket,
	U.FirstName + ' ' + U.LastName [Court]
FROM
	NeedHelpUheaa..DAT_Ticket T
	INNER JOIN NeedHelpUheaa..DAT_TicketsAssociatedUserID TA
		ON T.Ticket = TA.Ticket
	INNER JOIN CSYS..SYSA_DAT_Users U
		ON TA.SqlUserId = U.SqlUserId
	INNER JOIN CSYS..FLOW_DAT_Flow F
		ON T.TicketCode = F.FlowID
	INNER JOIN CSYS..FLOW_DAT_FlowStep FS
		ON F.FlowID = FS.FlowID
		AND T.[Status] = FS.[Status]
	INNER JOIN 
	(
		SELECT DISTINCT
			FlowId,
			MAX(FlowStepSequenceNumber) OVER (PARTITION BY FlowId) [MaxFlowStep]
		FROM
			CSYS..FLOW_DAT_FlowStep
		GROUP BY
			FlowID,
			FlowStepSequenceNumber
	) FSC
		ON F.FlowID = FSC.FlowID
WHERE
	TA.[Role] = 'Court'
	AND
	FS.FlowStepSequenceNumber != FSC.MaxFlowStep
	AND
	CAST(T.Requested AS DATE) < CAST(DATEADD(YEAR, -2, GETDATE()) AS DATE)
ORDER BY
	T.Ticket