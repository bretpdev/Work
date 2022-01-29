CREATE PROCEDURE [dbo].[spGENROperationsStaffingReport] 

AS

DECLARE @HeadHoncho			VARCHAR(50)
DECLARE @RETbl				TABLE (BUAndWINID		VARCHAR(200))

CREATE TABLE #TempRE (WINID	 	VARCHAR(50)) 

/* who is the manager of the operations division */
SET @HeadHoncho = (SELECT TOP 1  WindowsUserID 
			FROM dbo.GENR_REF_BU_Agent_Xref 
			WHERE BusinessUnit = 'Operations Division' AND Role = 'Manager')

/* get a list of all staff under manager of operations division */
INSERT INTO #TempRE (WINID) EXEC dbo.spGENRWhoIsInChargeOfWho @HeadHoncho 

/* join list of staff with GENR_REF_BU_Agent_Xref to figure out BU and combine to single ':' delimited field */
INSERT INTO @RETbl SELECT B.BusinessUnit + ';' + B.WindowsUserID AS RE
				FROM #TempRE A
				JOIN dbo.GENR_REF_BU_Agent_Xref B 
					ON B.WindowsUserID = A.WINID
					AND B.Role = 'Member Of'
				ORDER BY RE

SELECT * FROM @RETbl

DROP TABLE #TempRE