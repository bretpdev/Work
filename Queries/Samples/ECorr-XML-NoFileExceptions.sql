
DECLARE @File TABLE
(
	FName VARCHAR(256),
	StartedOn DATETIME
)

INSERT INTO
	@File
SELECT
	LEFT(RIGHT(PLM.LogMessage, LEN(PLM.LogMessage) - PATINDEX('%\E-Corr\%', PLM.LogMessage) - 7), LEN(RIGHT(PLM.LogMessage, LEN(PLM.LogMessage) - PATINDEX('%\E-Corr\%', PLM.LogMessage) - 7)) - 22),
	PL.StartedOn
FROM
	CLS.[log].ProcessLogMessages PLM
	INNER JOIN ProcessLogs.dbo.ProcessNotifications PN on PN.ProcessNotificationId = PLM.ProcessNotificationId
	INNER JOIN ProcessLogs.dbo.ProcessLogs PL on PL.ProcessLogId = PN.ProcessLogId
	INNER JOIN ProcessLogs.dbo.NotificationTypes NT on NT.NotificationTypeId = PN.NotificationTypeId
WHERE
	PL.ScriptId = 'E-Corr XML'
	AND 
	PL.StartedOn BETWEEN '2015-6-1' and GETDATE()
	AND
	PN.NotificationTypeId = 0 -- No File
--	AND
--	ISNUMERIC(LEFT(RIGHT(PLM.LogMessage, LEN(PLM.LogMessage) - PATINDEX('%\E-Corr\%', PLM.LogMessage) - 7), LEN(RIGHT(PLM.LogMessage, LEN(PLM.LogMessage) - PATINDEX('%\E-Corr\%', PLM.LogMessage) - 7)) - 22)) = 1
	/*PN.ProcessLogId in --today 7/28/2015 - 7/20/2015
		(
			278783,
			278123,
			277528,
			276526,
			275633			
		)
*/
SELECT DISTINCT
	--CAST(F.StartedOn as DATE) [FileDate]
	F.StartedOn [ProcessLoggedOn],
	L.Letter [SackerLetterId],
	DD.ADDR_ACCT_NUM [AccountNumber],
	DD.DocumentDetailsId,
	DD.Printed [MarkedAsPrintedOn],
	CASE WHEN CE.DI_VLD_CNC_EML_ADR = 1 AND CE.DI_CNC_ELT_OPI = 1 THEN 'Y' ELSE 'N' END [eCorrIndicator]
	
FROM
	@File F
	INNER JOIN ECorrFed.dbo.DocumentDetails DD ON DD.[Path] like '%' + F.FName + '%'
	INNER JOIN ECorrFed.dbo.Letters L on L.LetterId = DD.LetterId
	LEFT JOIN CDW.dbo.PH05_ContactEmail CE on CE.DF_SPE_ACC_ID = DD.ADDR_ACCT_NUM
WHERE
	L.Letter like 'TS%'
--GROUP BY
--	L.Letter
ORDER BY
	F.StartedOn,
	L.Letter,
	DD.ADDR_ACCT_NUM,
	DD.Printed
