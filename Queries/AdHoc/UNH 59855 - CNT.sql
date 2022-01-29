--total count should be 3:  2 updates and 1 insert	
	DECLARE @SASR INT = 4454



--only one row should be returned to be updated
SELECT
	COUNT(*) AS DAT_CNT
FROM 
	BSYS..SCKR_DAT_SASRequests	
WHERE 
	Request = @SASR

--only one row should be returned to be updated
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


--count of signature rows to be delted
/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
	COUNT(*) AS SIGN_CNT
FROM 
	[BSYS].[dbo].[SCKR_REF_UnitSign]
WHERE
	[Request] = @SASR
	AND [Class] = 'SAS'
	
	
		
	