---------------------------4 Years--------------------------------------
SELECT DISTINCT
	T.Ticket
FROM
	NeedHelpUheaa..DAT_Ticket T
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
		ON T.TicketCode = FSC.FlowID
WHERE
	FS.FlowStepSequenceNumber != FSC.MaxFlowStep
	AND
	CAST(T.Requested AS DATE) < CAST(DATEADD(YEAR, -4, GETDATE()) AS DATE)
ORDER BY
	T.Ticket


---------------------------2 Years--------------------------------------
SELECT DISTINCT
	T.Ticket
FROM
	NeedHelpUheaa..DAT_Ticket T
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
		ON T.TicketCode = FSC.FlowID
WHERE
	FS.FlowStepSequenceNumber != FSC.MaxFlowStep
	AND
	CAST(T.Requested AS DATE) < CAST(DATEADD(YEAR, -2, GETDATE()) AS DATE)
ORDER BY
	T.Ticket