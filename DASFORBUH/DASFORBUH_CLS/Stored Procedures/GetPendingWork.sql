CREATE PROCEDURE [dasforbuh].[GetPendingWork]
AS
	
	SELECT
		PQ.ProcessQueueId,
		PQ.AccountNumber,
		PQ.BeginDate,
		PQ.EndDate,
		FT.ActivityComment,
		PQ.ForbearanceAddedOn,
		PQ.ArcAddProcessingId,
		PQ.LD_DLQ_OCC,
		D.DelinquencyOverride
	FROM
		dasforbuh.ProcessQueue PQ
		INNER JOIN dasforbuh.ForbearanceTypes FT 
			ON FT.ForbearanceTypeId = PQ.ForbearanceTypeId
		INNER JOIN dasforbuh.Disasters D
			ON PQ.DisasterId = D.DisasterId
	WHERE
		DeletedOn IS NULL
		AND PQ.ArcAddProcessingId IS NULL


RETURN 0
