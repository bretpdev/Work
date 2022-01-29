
SELECT
	COALESCE(PLMC.LogMessage,PLMU.LogMessage) AS LogMessage,
	REPLACE(SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('Error Adding ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+13, 5),',','')AS Arc,
	REPLACE(SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('borrower: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+10, 10),',','')AS Account,
	SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('ScriptId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+10, 10) AS ScriptId,
	REPLACE(SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('ArcAddId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+10, 8),',','') AS ArcAddId,
	SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('UserId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+8, 7) AS UserId,
	CASE WHEN PLMC.ProcessLogMessageId IS NULL THEN 'UHEAA' ELSE 'CORNERSTONE' END AS Region
FROM 
	ProcessLogs..ProcessLogs PL
	INNER JOIN ProcessLogs..ProcessNotifications PN
		ON PN.ProcessLogId = PL.ProcessLogId
	LEFT OUTER JOIN CLS.[log].ProcessLogMessages PLMC
		ON PLMC.ProcessNotificationId = PN.ProcessNotificationId
	LEFT OUTER JOIN ULS.[log].ProcessLogMessages PLMU
		ON PLMU.ProcessNotificationId = PN.ProcessNotificationId
WHERE
	DATEDIFF(DAY, PL.StartedOn, GETDATE()) <= 30 
	AND PL.ScriptId = 'ARCADDPROC'
	AND COALESCE(PLMC.LogMessage,PLMU.LogMessage) LIKE '%Error adding%'
	AND SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('ScriptId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+10, 10) != 'BANADCR   '
ORDER BY
	SUBSTRING(COALESCE(PLMC.LogMessage,PLMU.LogMessage), CHARINDEX('ScriptId: ',COALESCE(PLMC.LogMessage,PLMU.LogMessage))+10, 10)
