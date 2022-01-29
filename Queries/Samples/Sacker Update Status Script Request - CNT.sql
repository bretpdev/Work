--total count should be 3:  2 updates and 1 insert	
	DECLARE @SR INT = 4935



--only one row should be returned to be updated
SELECT
	COUNT(*) AS DAT_CNT
FROM 
	BSYS..SCKR_DAT_ScriptRequests	
WHERE 
	Request = @SR

--only one row should be returned to be updated
SELECT
	COUNT(*) AS REF_CNT
FROM 
	BSYS..SCKR_REF_Status	
WHERE 
	Request = @SR
	AND [class] = 'Scr'
	AND [End] IS NULL


--only one row should be inserted
SELECT 1 AS INS_CNT