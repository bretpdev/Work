CREATE PROCEDURE [dbo].[spQCTR_QuarterlyRpt] 

@User					VARCHAR(50)

AS 

DECLARE @SDate			VARCHAR(12)
DECLARE @EDate			VARCHAR(12)

--begin and end date
IF (GETDATE() BETWEEN '01/01/' + CAST(YEAR(GETDATE()) as VARCHAR(4)) AND '03/31/' + CAST(YEAR(GETDATE()) as VARCHAR(4)))
BEGIN
	--if in the first quarter then report on the whole previous year
	SET @SDate = '01/01/' + CAST((YEAR(GETDATE()) - 1) as VARCHAR(4))
	SET @EDate = '12/31/' + CAST((YEAR(GETDATE()) - 1) as VARCHAR(4))
END
ELSE
BEGIN
	--report year to date if not in the first quarter
	SET @SDate = '01/01/' + CAST(YEAR(GETDATE()) as VARCHAR(4))
	SET @EDate = CONVERT(VARCHAR(12),GETDATE(),101)
END

CREATE TABLE #STAFF (WID VARCHAR(50))

INSERT INTO #STAFF EXEC dbo.spGENRWhoIsInChargeOfWho @User

SELECT A.Subject, B.UserID, E.FirstName, E.LastName, F.BusinessUnit, F.WindowsUserID, @SDate as StartDate, @EDate as EndDate, G.Status
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
LEFT JOIN  (
			SELECT BB.ISSUEID, BB.Status
			FROM (
				SELECT AAA.ISSUEID, MAX(AAA.Updated) as Updated
				FROM dbo.QCTR_DAT_Status AAA
				GROUP BY AAA.ISSUEID
				) AA
			INNER JOIN dbo.QCTR_DAT_Status BB
				ON AA.ISSUEID = BB.ISSUEID AND AA.Updated = BB.Updated 
				) G ON A.[ID] = G.ISSUEID
WHERE A.DateofActivity BETWEEN @SDate AND @EDate
ORDER BY F.BusinessUnit, B.UserID,  A.Subject