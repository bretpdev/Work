CREATE PROCEDURE [qstatsextr].[GetReportData]
	@RunTimeDate DATETIME,
	@BusinessUnit VARCHAR(50)
AS

	SELECT 
		QDET.[QueueName], 
		QDAT.RunTimeDate, 
		QDAT.Total, 
		QDAT.Complete, 
		QDAT.Critical, 
		QDAT.Cancelled, 
		QDAT.Outstanding, 
		QDAT.Problem, 
		QDAT.Late, 
		QDAT.Dept, 
		UDAT.UserID, 
		UDAT.StatusCode, 
		UDAT.CountInStatus, 
		UDAT.TotalTime, 
		UDAT.AvgTime,
		PREVIOUS.RunTimeDate AS NewTDS,
		CASE 
			WHEN DATEDIFF(dd, PREVIOUS.RunTimeDate, GETDATE()) = 0 THEN QDAT.Total - ISNULL(PREVIOUS.Total, 0)
			ELSE QDAT.Total - ISNULL(BEFOREYESTERDAY.Total, 0) - ISNULL(BEFOREYESTERDAY.Complete, 0) - ISNULL(BEFOREYESTERDAY.Cancelled, 0) 
		END [NewTotal], 
		QDAT.[Queue],  
		ISNULL(QDET.COMPASSShrtDesc, ' ') [ShortDesc]
	FROM 
		QSTA_LST_QueueDetail QDET
		INNER JOIN QSTA_DAT_QueueData QDAT
			ON QDET.QueueName = QDAT.[Queue]
		LEFT JOIN QSTA_DAT_UserData UDAT 
			ON QDET.QueueName = UDAT.[Queue] AND UDAT.RunTimeDate = QDAT.RunTimeDate 
		LEFT JOIN 
		(
			SELECT 
				X.[Queue], 
				X.RunTimeDate, 
				X.Total 
			FROM 
				QSTA_DAT_QueueData X
				INNER JOIN
				(
					SELECT 
						X.[Queue], 
						MAX(X.RunTimeDate)[RunTimeDate_max]
					FROM 
						QSTA_DAT_QueueData X
					WHERE
						X.RunTimeDate > DATEADD(DAY, -5, GETDATE())
						AND
						X.RunTimeDate != @RunTimeDate
					GROUP BY
						X.[Queue]
				) Dt on Dt.[Queue] = X.[Queue] AND X.RunTimeDate = Dt.RunTimeDate_max
		) PREVIOUS 
			ON PREVIOUS.[Queue] = QDAT.[Queue]
		LEFT JOIN 
		(
			SELECT
				Q.[Queue], 
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
						X.[Queue]
				) Dt ON Dt.[Queue] = Q.[Queue] AND Dt.RunTimeDate_max = Q.RunTimeDate
		) BEFOREYESTERDAY ON QDAT.[Queue] = BEFOREYESTERDAY.[Queue] 
	WHERE 
		QDET.BusinessUnit = @BusinessUnit
		AND
		QDAT.RunTimeDate = @RunTimeDate

RETURN 0
