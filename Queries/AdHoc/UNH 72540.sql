USE UDW
GO
--643
--INSERT INTO ULS.quecomplet.Queues([Queue], SubQueue, TaskControlNumber, ARC, AccountIdentifier, TaskStatusId, ActionResponseId, AddedAt, AddedBy)
SELECT DISTINCT
	
	WQ20.WF_QUE,
	WQ20.WF_SUB_QUE,
	WQ20.WN_CTL_TSK,
	WQ20.PF_REQ_ACT,
	PD10.DF_SPE_ACC_ID,
	7,
	1,
	GETDATE(),
	'UNH 72550'
FROM
	UDW..WQ20_TSK_QUE WQ20
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = WQ20.BF_SSN
	INNER JOIN
	(
		SELECT	
			BF_SSN,
			SUM(LA_CUR_PRI) AS BAL
		FROM
			UDW..LN10_LON
		GROUP BY
			BF_SSN
		HAVING SUM(LA_CUR_PRI) <= 0
	) LN10
		ON LN10.BF_SSN = WQ20.BF_SSN
WHERE 
	WF_QUE = 'RE'
	AND WF_SUB_QUE = '01'


