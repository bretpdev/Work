CREATE PROCEDURE [dasforbfed].[GetPendingWork]
AS
	
	SELECT
		PQ.ProcessQueueId,
		PQ.AccountNumber,
		PQ.BeginDate,
		PQ.EndDate,
		FT.ActivityComment,
		PQ.ForbearanceAddedOn,
		PQ.ArcAddProcessingId
	FROM
		dasforbfed.ProcessQueue PQ
		INNER JOIN dasforbfed.ForbearanceTypes FT 
			ON FT.ForbearanceTypeId = PQ.ForbearanceTypeId
	WHERE
		DeletedOn IS NULL
		AND PQ.ArcAddProcessingId IS NULL


RETURN 0
