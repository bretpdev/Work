
CREATE PROCEDURE [dbo].[spQSTA_GetReportData] 

@ReportFor	int,
@RT			datetime

AS

SELECT A.QueueName, 
	B.RunDateTime, 
	B.Total, 
	B.Complete, 
	B.Critical, 
	B.Canceled, 
	B.Outstanding, 
	B.Problem, 
	B.Late, 
	B.Dept, 
	C.UserID, 
	--C.StatusCode, 
	--C.CountInStatus, 
	--C.TotalTime, 
	--C.AvgTime, 
	D.RunDateTime AS NewTDS, 
	CASE 
		WHEN DATEDIFF(dd, D.RunDateTime, GETDATE()) = 0 THEN (B.Total - COALESCE(D.Total, 0)) 
		ELSE (B.Total - ((COALESCE(E.Total, 0) - COALESCE(E.Complete, 0)) - COALESCE(E.Canceled, 0))) 
	END AS NewTotal, 
	B.Queue,  
	COALESCE (A.COMPASSShrtDesc, ' ') AS ShortDesc
FROM QSTA_LST_QueueDetail A INNER JOIN QSTA_DAT_QueueData B ON A.QueueName = B.Queue 
LEFT OUTER JOIN (
			SELECT X.Queue, 
				X.RunDateTime, 
				X.Total 
			FROM QSTA_DAT_QueueData X 
			WHERE X.RunDateTime =  (
						SELECT MAX(Y.RunDateTime) 
						FROM QSTA_DAT_QueueData Y 
						WHERE Y.Queue = X.Queue 
							AND Y.RunDateTime <> @RT
						) 
			AND DATEDIFF(dd, X.RunDateTime, GETDATE()) < 5) D 
				ON B.Queue = D.Queue 
LEFT OUTER JOIN (
			SELECT Q.Queue, 
				Q.RunDateTime, 
				Q.Total, 
				Q.Complete,  
				Q.Canceled 
			FROM QSTA_DAT_QueueData Q 
			WHERE Q.RunDateTime = (
						SELECT MAX(R.RunDateTime) 
						FROM QSTA_DAT_QueueData R 
						WHERE R.Queue = Q.Queue 
						AND DATEDIFF(dd, R.RunDateTime, GETDATE()) <> 0
						) 
			AND DATEDIFF(dd, Q.RunDateTime, GETDATE()) < 5) E 
				ON B.Queue = E.Queue 
LEFT OUTER JOIN QSTA_DAT_UserData C 
		ON A.QueueName = C.Queue AND C.RunDateTime = B.RunDateTime 
WHERE A.BusinessUnit = @ReportFor AND B.RunDateTime = @RT


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spQSTA_GetReportData] TO [db_executor]
    AS [dbo];

