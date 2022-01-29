UPDATE
	PN
SET
	PN.ResolvedAt = GETDATE(),
	PN.ResolvedBy = 'CNH '
FROM
	CLS.[log].ProcessLogMessages PLM
	LEFT JOIN ProcessLogs..ProcessNotifications PN
		ON PN.ProcessNotificationId = PLM.ProcessNotificationId
	LEFT JOIN ProcessLogs..ProcessLogs PL
		ON PL.ProcessLogId = PN.ProcessLogId
	LEFT JOIN
		(
			SELECT
				DF_SPE_ACC_ID,
				DF_PRS_ID
			FROM
				CDW..PDXX_PRS_NME
		) PDXX
			ON PDXX.DF_SPE_ACC_ID = CLS.dbo.SplitAndRemoveQuotes(CLS.dbo.SplitAndRemoveQuotes(PLM.LogMessage, ':', X, X), ',', X, X)
	LEFT JOIN
		(
			SELECT
				DISTINCT BF_SSN
			FROM
				CDW..CS_Transfer_EAXX
		) CS
			ON CS.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PLM.LogMessage LIKE '%ascra%'
	AND PL.Region = 'cornerstone'
	AND CAST(PL.StartedOn AS DATE) > 'XX/X/XXXX'
	AND PN.ResolvedAt IS NULL