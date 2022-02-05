CREATE PROCEDURE dbo.spQCTR_MonthlyRpt 

@User					VARCHAR(50)

AS 

DECLARE @SDate			VARCHAR(12)
DECLARE @EDate			VARCHAR(12)

--figure out start date of month being reported for
SET @SDate = CONVERT(VARCHAR(12),DATEADD(d,-(day(GETDATE()) - 1), DATEADD(mm,-1,GETDATE())),101)
--figure out end date of month being reported for
SET @EDate = CONVERT(VARCHAR(12),DATEADD(d,-(day(GETDATE())), GETDATE()),101)

CREATE TABLE #STAFF (WID VARCHAR(50))

INSERT INTO #STAFF EXEC dbo.spGENRWhoIsInChargeOfWho @User

SELECT A.Subject, B.UserID, E.FirstName, E.LastName, F.BusinessUnit, F.WindowsUserID, @SDate as StartDate, @EDate as EndDate
FROM dbo.QCTR_DAT_Issue A
INNER JOIN dbo.QCTR_DAT_Responsible B
	ON A.ID = B.IssueID
INNER JOIN dbo.SYSA_LST_UserIDInfo C
	ON B.UserID = C.UserID
INNER JOIN #STAFF D
	ON C.WindowsUserName = D.WID
INNER JOIN dbo.SYSA_LST_Users E
	ON C.WindowsUserName = E.WindowsUserName
INNER JOIN dbo.GENR_REF_BU_Agent_Xref F
	ON C.WindowsUserName = F.WindowsUserID AND Role = 'Member Of'
WHERE DATEDIFF(M, Getdate(), A.DateofActivity) = -1
ORDER BY F.BusinessUnit, B.UserID,  A.Subject