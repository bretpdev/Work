SELECT
	*
FROM
	[UDW].[dbo].[WQ20_TSK_QUE]
WHERE
	WF_QUE = '4X'
	AND 
	(
		(WX_MSG_1_TSK LIKE '%0202%' AND WX_MSG_1_TSK LIKE '%lender code%')
		OR (WX_MSG_1_TSK LIKE '%0803%' AND WX_MSG_1_TSK LIKE '%1st disb%')
		OR (WX_MSG_1_TSK LIKE '%1301%' AND WX_MSG_1_TSK LIKE '%invalid loan%')
		OR (WX_MSG_1_TSK LIKE '%1402%' AND WX_MSG_1_TSK LIKE '%loan status%')
		OR (WX_MSG_1_TSK LIKE '%1702%' AND WX_MSG_1_TSK LIKE '%dfr end dte%')
		OR (WX_MSG_1_TSK LIKE '%1901%' AND WX_MSG_1_TSK LIKE '%invalid dt%')
		OR (WX_MSG_1_TSK LIKE '%2101%' AND WX_MSG_1_TSK LIKE '%invalid date%')
		OR (WX_MSG_1_TSK LIKE '%2502%' AND WX_MSG_1_TSK LIKE '%responsibility date%')
	)