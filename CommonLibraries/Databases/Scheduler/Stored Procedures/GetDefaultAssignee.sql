CREATE PROCEDURE GetDefaultAssignee
(
	@Dev BIT,
	@RequestType VARCHAR(6)
)
AS

IF(@Dev = 1)
BEGIN
	DECLARE @temp TABLE(Developer VARCHAR(40), completionDate DATETIME)
	INSERT INTO @temp (Developer, completionDate)
	SELECT LT.SOC, ISNULL(MAX(LT.SetupEstimateEnd),GETDATE())
	FROM [BSYS].[dbo].[LTDB_DAT_Requests] LT 
		INNER JOIN UserRequestTypes A ON A.UserId = LT.SOC
		INNER JOIN RequestTypes R ON R.RequestTypeId = A.RequestTypeId
	WHERE R.RequestType = @RequestType AND A.Developer = @Dev
	GROUP BY LT.SOC
	
	INSERT INTO @temp (Developer, completionDate)
	SELECT RP.Programmer, ISNULL(MAX(SR.DevEstimateEnd),GETDATE())
	FROM [BSYS].[dbo].[SCKR_DAT_ScriptRequests] SR
		INNER JOIN [BSYS].[dbo].[SCKR_REF_Programmer] RP ON SR.Request = RP.Request
		INNER JOIN UserRequestTypes A ON A.UserId = RP.Programmer
		INNER JOIN RequestTypes R ON R.RequestTypeId = A.RequestTypeId
	WHERE R.RequestType = @RequestType AND A.Developer = @Dev
	GROUP BY RP.Programmer
	
	INSERT INTO @temp (Developer, completionDate)
	SELECT RP.Programmer, ISNULL(MAX(SASR.DevEstimateEnd),GETDATE())
	FROM [BSYS].[dbo].[SCKR_DAT_SASRequests] SASR
		INNER JOIN [BSYS].[dbo].[SCKR_REF_Programmer] RP ON SASR.Request = RP.Request
		INNER JOIN UserRequestTypes A ON A.UserId = RP.Programmer
		INNER JOIN RequestTypes R ON R.RequestTypeId = A.RequestTypeId
	WHERE R.RequestType = @RequestType AND A.Developer = @Dev
	GROUP BY RP.Programmer
	
	DECLARE @temp2 TABLE(Developer VARCHAR(40), completionDate DATETIME)
	INSERT INTO @temp2 (Developer, completionDate)
	SELECT T.Developer, MAX(T.completionDate) 
	FROM @temp T 
	GROUP BY T.Developer
	
	SELECT TOP 1 T.Developer 
	FROM @temp2 T 
	WHERE T.completionDate = (SELECT MIN(completionDate) FROM @temp2) 
END

IF(@Dev = 0)
BEGIN
	DECLARE @temp3 TABLE(Tester VARCHAR(40), completionDate DATETIME)
	INSERT INTO @temp3 (Tester, completionDate)
	SELECT LT.Tester, ISNULL(MAX(LT.TestEstimateEnd),GETDATE())
	FROM [BSYS].[dbo].[LTDB_DAT_Requests] LT 
		INNER JOIN UserRequestTypes A ON A.UserId = LT.Tester
		INNER JOIN RequestTypes R ON R.RequestTypeId = A.RequestTypeId
	WHERE R.RequestType = @RequestType AND A.Developer = @Dev
	GROUP BY LT.Tester
	
	INSERT INTO @temp3 (Tester, completionDate)
	SELECT SR.SSA, ISNULL(MAX(SR.TestEstimateEnd),GETDATE())
	FROM [BSYS].[dbo].[SCKR_DAT_ScriptRequests] SR
		INNER JOIN UserRequestTypes A ON A.UserId = SR.SSA
		INNER JOIN RequestTypes R ON R.RequestTypeId = A.RequestTypeId
	WHERE R.RequestType = @RequestType AND A.Developer = @Dev
	GROUP BY SR.SSA
	
	INSERT INTO @temp3 (Tester, completionDate)
	SELECT SASR.SSA, ISNULL(MAX(SASR.TestEstimateEnd),GETDATE())
	FROM [BSYS].[dbo].[SCKR_DAT_SASRequests] SASR
		INNER JOIN UserRequestTypes A ON A.UserId = SASR.SSA
		INNER JOIN RequestTypes R ON R.RequestTypeId = A.RequestTypeId
	WHERE R.RequestType = @RequestType AND A.Developer = @Dev
	GROUP BY SASR.SSA
	
	DECLARE @temp4 TABLE(Tester VARCHAR(40), completionDate DATETIME)
	INSERT INTO @temp4 (Tester, completionDate)
	SELECT T.Tester, MAX(T.completionDate) 
	FROM @temp3 T 
	GROUP BY T.Tester
	
	SELECT TOP 1 T.Tester 
	FROM @temp4 T 
	WHERE T.completionDate = (SELECT MIN(completionDate) FROM @temp4) 
END

