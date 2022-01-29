CREATE PROCEDURE [compcb].[AddNewTasks]
AS
	INSERT INTO ULS.compcb.ProcessingQueue (BorrowerSsn, BorrowerAccountNumber, EndorserSsn, EndorserAccountNumber, TaskControlNumber, RequestArc, TaskRequestedDate, IsEndorserTask, IsForeignAddress)
	SELECT DISTINCT
		WQ20.BF_SSN [BorrowerSsn],
		PD10.DF_SPE_ACC_ID [BorrowerAccountNumber],
		CASE
			WHEN AY10.LC_ATY_RCP = 'E' THEN AY10.LF_ATY_RCP
			ELSE NULL
			END AS [EndorserSsn],
		CASE
			WHEN AY10.LC_ATY_RCP = 'E' THEN (SELECT DF_SPE_ACC_ID FROM UDW..PD10_PRS_NME WHERE DF_PRS_ID = AY10.LF_ATY_RCP)
			ELSE NULL
		END AS [EndorserAccountNumber],
		WQ20.WN_CTL_TSK [TaskControlNumber],
		WQ20.PF_REQ_ACT [RequestArc], 
		WQ20.WD_ACT_REQ [TaskRequestedDate],
		CASE
			WHEN WQ20.PF_REQ_ACT = 'S3CT1' THEN CAST(1 AS BIT)
			WHEN WQ20.PF_REQ_ACT = 'S3CTB' THEN CAST(0 AS BIT)
			ELSE NULL
		END AS [IsEndorserTask],
		0 [IsForeignAddress] -- The C# script checks the session to see if it's a foreign address then updates this field
	FROM
		UDW..WQ20_TSK_QUE WQ20
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = WQ20.BF_SSN
		INNER JOIN UDW..AY10_BR_LON_ATY AY10
			ON AY10.BF_SSN = WQ20.BF_SSN
			AND AY10.LN_ATY_SEQ = WQ20.LN_ATY_SEQ
			AND AY10.LC_STA_ACTY10 = 'A'
		LEFT JOIN ULS.compcb.ProcessingQueue PQ
			ON PQ.TaskControlNumber = WQ20.WN_CTL_TSK
			AND PQ.RequestArc = WQ20.PF_REQ_ACT
			AND PQ.TaskRequestedDate = WQ20.WD_ACT_REQ
			AND PQ.DeletedAt IS NULL
			AND (PQ.ProcessedAt IS NULL OR CAST(PQ.ProcessedAt AS DATE) = CAST(GETDATE() AS DATE))
	WHERE
		WF_QUE = 'JH'
		AND WF_SUB_QUE = '01'
		AND WC_STA_WQUE20 = 'U'
		AND PQ.BorrowerSsn IS NULL

RETURN 0
