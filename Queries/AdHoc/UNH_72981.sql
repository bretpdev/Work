USE ULS
GO

UPDATE PN
SET
	PN.ResolvedAt = GETDATE(),
	PN.ResolvedBy = 'UNH 72981'
--SELECT
--	aap.*,
--	pn.ProcessNotificationId,
--	pn.ResolvedBy,
--	pn.ResolvedBy
FROM
	ArcAddProcessing AAP
	INNER JOIN ArcAddProcessLoggerMapping MAP 
		ON MAP.ArcAddProcessingId = AAP.ArcAddProcessingId
	INNER JOIN ProcessLogs..ProcessNotifications PN 
		ON PN.ProcessLogId = MAP.ProcessLogId 
		AND PN.ProcessNotificationId = MAP.ProcessNotificationId
    INNER JOIN ULS.[log].ProcessLogMessages PLM 
		ON PLM.ProcessNotificationId = PN.ProcessNotificationId
	LEFT JOIN ODW..PD01_PDM_INF PD01
		ON PD01.DF_SPE_ACC_ID = AAP.AccountNumber
WHERE
	 AAP.CreatedAt > CentralData.dbo.AddBusinessDays(GETDATE(), -10)
     AND AAP.ProcessOn < GETDATE()
     AND AAP.ProcessingAttempts >= 2
     AND PN.NotificationSeverityTypeId != 0
     AND PN.ResolvedBy IS NULL
     AND AAP.LN_ATY_SEQ IS NULL
     AND aap.ARC = 'KATNY'
	 AND PD01.DF_PRS_ID IS NULL