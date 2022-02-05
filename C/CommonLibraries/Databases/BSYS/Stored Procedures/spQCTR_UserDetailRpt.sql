

CREATE PROCEDURE [dbo].[spQCTR_UserDetailRpt] 

@Mode					VARCHAR(8000),
@User					VARCHAR(50),
@RptForUserID				VARCHAR(50)

AS 

DECLARE @SDate			VARCHAR(12)
DECLARE @EDate			VARCHAR(12)

--figure out start date of month being reported for
SET @SDate = CONVERT(VARCHAR(12),DATEADD(d,-(day(GETDATE()) - 1), DATEADD(mm,-1,GETDATE())),101)
--figure out end date of month being reported for
SET @EDate = CONVERT(VARCHAR(12),DATEADD(d,-(day(GETDATE())), GETDATE()),101)

EXEC ('SELECT DISTINCT A.[ID], 
	A.SUBJECT,
	CONVERT(VARCHAR(12),A.DateofActivity,101) as DateofActivity, 
	''' + @SDate + ''' as StartDate,  
	''' + @EDate + ''' as EndDate, 
	''' + @RptForUserID + ''' as RptForUserID 
FROM QCTR_DAT_Issue A
LEFT JOIN  (
		SELECT BB.ISSUEID, BB.Status
		FROM (
			SELECT AAA.ISSUEID, MAX(AAA.Updated) as Updated
			FROM dbo.QCTR_DAT_Status AAA
			GROUP BY AAA.ISSUEID
			) AA
		INNER JOIN dbo.QCTR_DAT_Status BB
			ON AA.ISSUEID = BB.ISSUEID AND AA.Updated = BB.Updated 
			) B ON A.[ID] = B.ISSUEID
LEFT JOIN dbo.QCTR_DAT_Court C
	ON A.[ID] = C.ISSUEID
	AND C.Ended IS NULL
INNER JOIN dbo.GENR_REF_BU_Agent_Xref E
	ON E.Role = ''Authorized QC'' AND E.WindowsUserID = ''' + @User + '''
INNER JOIN dbo.QCTR_DAT_Responsible F
	ON A.[ID] = F.ISSUEID
WHERE ' + @Mode +  '  ORDER BY A.[ID]')

--BUSINESS UNIT LOGIC WAS BREAKING
--EXEC ('SELECT DISTINCT A.[ID], 
--	A.SUBJECT,
--	CONVERT(VARCHAR(12),A.DateofActivity,101) as DateofActivity, 
--	''' + @SDate + ''' as StartDate,  
--	''' + @EDate + ''' as EndDate, 
--	''' + @RptForUserID + ''' as RptForUserID 
--FROM QCTR_DAT_Issue A
--LEFT JOIN  (
--		SELECT BB.ISSUEID, BB.Status
--		FROM (
--			SELECT AAA.ISSUEID, MAX(AAA.Updated) as Updated
--			FROM dbo.QCTR_DAT_Status AAA
--			GROUP BY AAA.ISSUEID
--			) AA
--		INNER JOIN dbo.QCTR_DAT_Status BB
--			ON AA.ISSUEID = BB.ISSUEID AND AA.Updated = BB.Updated 
--			) B ON A.[ID] = B.ISSUEID
--LEFT JOIN dbo.QCTR_DAT_Court C
--	ON A.[ID] = C.ISSUEID
--	AND C.Ended IS NULL
--LEFT JOIN dbo.QCTR_DAT_BU D
--	ON A.[ID] = D.ISSUEID
--INNER JOIN dbo.GENR_REF_BU_Agent_Xref E
--	ON D.BU = E.BusinessUnit  AND  E.Role = ''Authorized QC'' AND E.WindowsUserID = ''' + @User + '''
--INNER JOIN dbo.QCTR_DAT_Responsible F
--	ON A.[ID] = F.ISSUEID
--WHERE ' + @Mode +  '  ORDER BY A.[ID]')