CREATE PROCEDURE nsldsconso.[GetMostRecentDataLoadRun]
AS

	SELECT TOP 1
		DLR.DataLoadRunId,
		DLR.StartedOn,
		DLR.StartedBy,
		DLR.EndedOn,
		DLR.BorrowerCount,
		DLR.[Filename],
		COUNT(B.Ssn) ActualBorrowerCount,
		CASE
			WHEN MAX(B.AddedOn) >= DLR.EndedOn AND MAX(B.AddedOn) >= DLR.StartedOn THEN MAX(B.AddedOn)
			WHEN DLR.EndedOn >= DLR.StartedOn THEN DLR.EndedOn
			ELSE DLR.StartedOn
		END LastUpdated
	FROM
		nsldsconso.DataLoadRuns DLR
		LEFT JOIN nsldsconso.Borrowers B
			ON B.DataLoadRunId = DLR.DataLoadRunId
	GROUP BY
		DLR.dataLoadRunId,
		DLR.StartedOn,
		DLR.StartedBy,
		DLR.EndedOn,
		DLR.BorrowerCount,
		DLR.[Filename]
	ORDER BY
		DLR.DataLoadRunId DESC


RETURN 0
