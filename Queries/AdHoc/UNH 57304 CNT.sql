--total count should be 3:  2 updates and 1 insert	
	DECLARE @SASR INT = 2897



--only one row should be returned
SELECT
	COUNT(*) AS DAT_CNT
FROM 
	BSYS..SCKR_DAT_SASRequests	
WHERE 
	Request = @SASR

--only one row should be returned
SELECT
	COUNT(*) AS REF_CNT
FROM 
	BSYS..SCKR_REF_Status	
WHERE 
	Request = @SASR
	AND [class] = 'SAS'
	AND [End] IS NULL


--only one row should be inserted
SELECT 1 AS INS_CNT