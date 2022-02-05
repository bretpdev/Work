
CREATE PROCEDURE [dbo].[spQSTA_GetReportData]
(
	@ReportFor	varchar(50),
	@RT		datetime
)
AS

SELECT 
	B.Queue, 
	B.RunTimeDate, 
	B.Total, 
	B.Complete, 
	B.Critical, 
	B.Cancelled, 
	B.Outstanding, 
	B.Problem, 
	B.Late, 
	B.Dept, 
	C.UserID, 
	C.StatusCode, 
	C.CountInStatus, 
	C.TotalTime, 
	C.AvgTime,
	D.RunTimeDate AS NewTDS,
	CASE 
		WHEN DATEDIFF(dd, D.RunTimeDate, GETDATE()) = 0 THEN B.Total - ISNULL(D.Total, 0)
		ELSE B.Total - ISNULL(E.Total, 0) - ISNULL(E.Complete, 0) - ISNULL(E.Cancelled, 0) 
	END [NewTotal], 
	B.Queue,  
	ISNULL(A.COMPASSShrtDesc, ' ') [ShortDesc]
FROM 
	QSTA_LST_QueueDetail A
	INNER JOIN QSTA_DAT_QueueData B ON A.QueueName = B.Queue 
	LEFT JOIN 
	(
		SELECT 
			X.Queue, 
			X.RunTimeDate, 
			X.Total 
		FROM 
			QSTA_DAT_QueueData X
			INNER JOIN
			(
				SELECT 
					X.Queue, 
					MAX(X.RunTimeDate)[RunTimeDate_max]
				FROM 
					QSTA_DAT_QueueData X
				WHERE
					X.RunTimeDate > DATEADD(DAY, -5, GETDATE())
					AND
					X.RunTimeDate != @RT
				GROUP BY
					X.Queue
			) Dt on Dt.Queue = X.Queue AND X.RunTimeDate = Dt.RunTimeDate_max
	) D on D.Queue = B.Queue
	LEFT JOIN 
	(
		SELECT
			Q.Queue, 
			Q.RunTimeDate, 
			Q.Total, 
			Q.Complete,  
			Q.Cancelled 
		FROM 
			QSTA_DAT_QueueData Q
			INNER JOIN
			(
				SELECT
					X.Queue, 
					MAX(X.RunTimeDate) [RunTimeDate_max]
				FROM 
					QSTA_DAT_QueueData X
				WHERE
					CAST(X.RunTimeDate as DATE) BETWEEN DATEADD(DAY, -5, CAST(GETDATE() as DATE)) AND DATEADD(DAY, -1, CAST(GETDATE() as DATE))
				GROUP BY
					X.Queue
			) Dt ON Dt.Queue = Q.Queue AND Dt.RunTimeDate_max = Q.RunTimeDate
	) E ON B.Queue = E.Queue 
	LEFT JOIN QSTA_DAT_UserData C ON A.QueueName = C.Queue AND C.RunTimeDate = B.RunTimeDate 
WHERE 
	A.BusinessUnit = @ReportFor 
	AND
	B.RunTimeDate = @RT