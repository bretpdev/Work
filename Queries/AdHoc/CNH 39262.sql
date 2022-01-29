/****** Script for SelectTopNRows command from SSMS  ******/
SELECT DISTINCT 
	sc.Script,
	sc.ID AS SCRIPT_ID,
	S.Job AS SAS_JOB_NAME,
	S.ID AS SAS_ID
FROM 
	[BSYS]..[SCKR_DAT_SCRIPTS] SC
	INNER JOIN
	(
		SELECT 
			[Title],
			[Script]
		FROM 
			[BSYS].[dbo].[SCKR_DAT_ScriptRequests]
		WHERE 
			CurrentStatus = 'Complete'
			
	)REQ
		ON REQ.Script = SC.Script
--	INNER JOIN 
--	(
--		SELECT 

--*
     
   
--  FROM [BSYS].[dbo].[SCKR_DAT_ScriptRequests]
--  WHERE CurrentStatus = 'Complete'
--  AND
--  (
--  Title LIKE '%RETIRE%'
--  OR
--  Summary LIKE '%RETIRE%'
--  )
--	) RET
--		ON RET.Script = SC.Script
	LEFT JOIN [BSYS]..[SCKR_REF_SASSCRIPT] RS
		ON SC.Script = RS.Script
	LEFT JOIN BSYS..SCKR_DAT_SAS S
		ON RS.Job = S.Job
WHERE
	SC.[Status] = 'Active'
	AND SC.Script NOT LIKE '%FED%'


/****** Script for SelectTopNRows command from SSMS  ******/
SELECT DISTINCT 
	sc.Job,
	sc.ID AS SCRIPT_ID
FROM 
	[BSYS]..SCKR_DAT_SAS SC
	INNER JOIN
	(
		SELECT 
			[Title],
			Job
		FROM 
			[BSYS].[dbo].SCKR_DAT_SASRequests
		WHERE 
			CurrentStatus = 'Complete'
			
	)REQ
		ON REQ.Job = SC.Job
WHERE
	SC.[Status] = 'Active'
	and 
	(
		sc.id like 'UTL%'
		or
		sc.Job not like '%FED%'
	)
	--AND SC.Script LIKE '%FED%'
