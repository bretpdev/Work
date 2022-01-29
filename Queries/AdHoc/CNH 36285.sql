SELECT DISTINCT	
	EML.Arc,
	EML.LetterId AS EML_LetterId
FROM
	CLS.emailbtcf.EmailCampaigns EML
WHERE
	EML.Arc IN
		(
		'DDEML',
		'DQEXX',
		'DQEXX',
		'DQEXX',
		'DQEXX',
		'DQEXX',  
		'DQEXX', 
		'DQEXX',  
		'DQEXX', 
		'DQEXX', 
		'DQEXX', 
		'DQEXX',
		'DQEXX', 
		'DQEXX',
		'EMLXX',
		'ERPMT',
		'EXFBD'
		)
ORDER BY
	EML.Arc


SELECT DISTINCT
	ACXX.PF_REQ_ACT,
	ACXX.PF_LTR AS ACXX_PF_LTR
FROM
	CDW..ACXX_ACT_REQ_LTR ACXX
WHERE
	ACXX.PF_REQ_ACT IN
		(
		'MSDXX', 
		'CODFP', 
		'DLXXX', 
		'EZPAY', 
		'DLXXX', 
		'DLXXX', 
		'DAIDR', 
		'CRPRE'
		)
ORDER BY
	ACXX.PF_REQ_ACT
		
SELECT DISTINCT
	ARCS.Arc,
	LTR.Letter AS CLS_PRINT_ARCS
FROM
	CLS.[print].Arcs ARCS
	INNER JOIN CLS.[print].ArcScriptDataMapping SDM
		ON ARCS.ArcId = SDM.ArcId
	INNER JOIN CLS.[print].ScriptData SD
		ON SDM.ScriptDataId = SD.ScriptDataId
	INNER JOIN CLS.[print].Letters LTR
		ON SD.LetterId =  LTR.LetterId
WHERE
	ARCS.Arc IN
		(
		'MSDXX', 
		'CODFP', 
		'DLXXX', 
		'EZPAY', 
		'DLXXX', 
		'DLXXX', 
		'DAIDR', 
		'CRPRE'
		)
ORDER BY
	ARCS.Arc