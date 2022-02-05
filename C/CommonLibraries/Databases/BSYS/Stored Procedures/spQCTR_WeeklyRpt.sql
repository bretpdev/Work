CREATE PROCEDURE [dbo].[spQCTR_WeeklyRpt] 

@User					VARCHAR(50)

AS 

CREATE TABLE #STAFF (WID VARCHAR(50))

INSERT INTO #STAFF EXEC dbo.spGENRWhoIsInChargeOfWho @User

--SELECT * FROM #STAFF

SELECT A.ID, F.FirstName + ' ' + F.LastName AS UserName, A.Requested, A.Priority 
FROM dbo.QCTR_DAT_Issue A
INNER JOIN  dbo.QCTR_DAT_Status B 
	ON A.[ID] = B.ISSUEID
INNER JOIN (
				SELECT AA.ISSUEID, BB.WindowsUserName
				FROM dbo.QCTR_DAT_Responsible AA
				INNER JOIN dbo.SYSA_LST_UserIDInfo BB
					ON AA.UserID = BB.UserID
			) C ON A.[ID] = C.ISSUEID
INNER JOIN dbo.SYSA_LST_Users F
	ON C.WindowsUserName = F.WindowsUserName
INNER JOIN #STAFF G	
	ON C.WindowsUserName = G.WID
WHERE DATEDIFF(ww, A.Requested, GETDATE()) = 1 AND --logged last week
		B.Ended IS NULL AND --current status should have a null ended date
		B.Status = 'Discussion' --status is discussion
ORDER BY  A.[ID]