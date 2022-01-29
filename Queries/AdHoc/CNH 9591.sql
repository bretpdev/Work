UPDATE
	LTXX
SET 
	PrintedAt = NULL,
	InactivatedAt = NULL,
	SystemLetterExclusionReasonId = NULL
FROM
(
	SELECT
		SUBSTRING(CPLM.LogMessage, CHARINDEX('borrower: ', CPLM.LogMessage, X) + XX, XX) AS Account,
		SUBSTRING(CPLM.LogMessage, CHARINDEX('letter id ', CPLM.LogMessage, X) + XX, XX) AS LetterId
	FROM 
		ProcessLogs..ProcessLogs PL
		INNER JOIN ProcessLogs..ProcessNotifications PN
			ON PN.ProcessLogId = PL.ProcessLogId
		LEFT JOIN CLS.log.ProcessLogMessages CPLM
			ON CPLM.ProcessNotificationId = PN.ProcessNotificationId
	WHERE 
		PL.ScriptId = 'ECORRSLFED'
		AND DATEDIFF(DAY,PL.StartedOn,GETDATE()) = X
) PL 
	INNER JOIN CDW..LTXX_LetterRequests LTXX
		ON PL.Account = LTXX.DF_SPE_ACC_ID
		AND LTRIM(RTRIM(PL.LetterId)) = LTXX.RM_DSC_LTR_PRC
WHERE 
	(DATEDIFF(DAY,LTXX.PrintedAt,GETDATE()) = X OR DATEDIFF(DAY,InactivatedAt,GETDATE()) = X) 
	AND DATEDIFF(day, LTXX.RT_RUN_SRT_DTS_PRC, GETDATE()) <= X
