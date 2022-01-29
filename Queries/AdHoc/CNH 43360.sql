USE CDW
GO
--SELECT
--*
--FROM
--(
SELECT distinct
	PDXX.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	WF_QUE AS [QUEUE],
	PF_REQ_ACT AS ARC,
	COUNT(*) OVER (PARTITION BY PDXX.DF_SPE_ACC_ID,	WF_QUE,	PF_REQ_ACT) AS TASK_COUNT
	--COUNT(*) OVER (PARTITION BY PDXX.DF_SPE_ACC_ID) AS BWR_COUNT
FROM
	CDW..WQXX_TSK_QUE WQXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = WQXX.BF_SSN
WHERE
	WF_QUE = 'XC'
	AND WC_STA_WQUEXX NOT IN ('C','X')
--) P
--WHERE P.BWR_COUNT != P.TASK_COUNT
