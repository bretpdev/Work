
CREATE PROCEDURE [dbo].[spQCTR_User12MonthRpt] 

@UserID		VARCHAR(10),
@Subject		VARCHAR(100)

AS

DECLARE @Counter	INT
DECLARE @TheMonth	VARCHAR(10)
DECLARE @TempTable	TABLE (
				RecNum		INT IDENTITY (1,1),
				TheMonths	VARCHAR(10),
				TheCount	INT
				)
DECLARE @FinalTable	TABLE (
				RecNum		INT IDENTITY (1,1),
				TheMonths	VARCHAR(10),
				TheCount	INT
				)

SET @Counter = 12

INSERT INTO @TempTable (TheMonths, TheCount) (SELECT  CASE
														WHEN MONTH(A.Requested) < 10 THEN '0' + CAST(MONTH(A.Requested) AS VARCHAR(2)) + '/' + CAST(YEAR(A.Requested) AS VARCHAR(4))
														ELSE CAST(MONTH(A.Requested) AS VARCHAR(2)) + '/' + CAST(YEAR(A.Requested) AS VARCHAR(4))
													END as TheMonths, 
													COUNT(*) as TheCount
												FROM dbo.QCTR_DAT_Issue A
												INNER JOIN dbo.QCTR_DAT_Responsible B
													ON A.[ID] = B.IssueID AND B.UserID = @UserID
												WHERE A.Subject = @Subject 
												AND DATEDIFF(M,GETDATE(),A.Requested) > -14
												GROUP BY CASE
													WHEN MONTH(A.Requested) < 10 THEN '0' + CAST(MONTH(A.Requested) AS VARCHAR(2)) + '/' + CAST(YEAR(A.Requested) AS VARCHAR(4))
													ELSE CAST(MONTH(A.Requested) AS VARCHAR(2)) + '/' + CAST(YEAR(A.Requested) AS VARCHAR(4))
												END
												)

WHILE @Counter <> -1
BEGIN
	--put month at front
	IF MONTH(DATEADD(M, @Counter * -1,GETDATE())) >= 10
	BEGIN
		SET @TheMonth = CAST(MONTH(DATEADD(M, @Counter * -1,GETDATE())) AS VARCHAR(2))
	END
	ELSE
	BEGIN
		SET @TheMonth = '0' + CAST(MONTH(DATEADD(M, @Counter * -1,GETDATE())) AS VARCHAR(2))
	END
	--add year to the end of the month value
	SET @TheMonth = @TheMonth + '/' + CAST(YEAR(DATEADD(M, @Counter * -1,GETDATE())) as VARCHAR(4))
	IF (SELECT COUNT(*) FROM @TempTable WHERE TheMonths = @TheMonth) = 1
	BEGIN
		--add total to table if found
		INSERT INTO @FinalTable (TheMonths, TheCount) (
									SELECT TheMonths, TheCount FROM @TempTable WHERE TheMonths = @TheMonth
								)
	END
	ELSE
	BEGIN
		--insert zero total if not found in table
		INSERT INTO @FinalTable (TheMonths, TheCount) VALUES (@TheMonth,0)
	END
	SET @Counter = @Counter -1 
END

SELECT TheMonths, TheCount 
FROM @FinalTable
ORDER BY RecNum