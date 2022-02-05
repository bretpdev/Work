CREATE PROCEDURE [dbo].[spQCTR_DailyRpt] 

@User					VARCHAR(50)

AS 

CREATE TABLE #STAFF (WID VARCHAR(50))

INSERT INTO #STAFF EXEC dbo.spGENRWhoIsInChargeOfWho @User

--SELECT * FROM #STAFF

SELECT A.ID, F.FirstName + ' ' + F.LastName AS UserName, A.Priority  
FROM dbo.QCTR_DAT_Issue A
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
WHERE DATEDIFF(dd, A.Requested, GETDATE()) = 1 --logged yesterday
ORDER BY  A.[ID]