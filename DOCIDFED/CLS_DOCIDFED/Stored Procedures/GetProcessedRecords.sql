CREATE PROCEDURE [docid].[GetProcessedRecords]
	@SelectedDate datetime
AS
	SELECT
		DP.AccountIdentifier,
		D.Document,
		S.[Source],
		AAP.ProcessedAt,
		AAP.ArcAddProcessingId,
		DP.AddedBy,
		DP.AddedAt,
		AAP.Comment
	FROM
		docid.DocumentsProcessed DP
		LEFT JOIN docid.Documents D
			ON DP.DocumentsId = D.DocumentsId
		LEFT JOIN ArcAddProcessing AAP
			ON DP.ArcAddProcessingId = AAP.ArcAddProcessingId
		LEFT JOIN Sources S
			ON DP.SourceId = S.SourceId
	WHERE
		CAST(AddedAt AS DATE) = CAST(@SelectedDate AS DATE)

RETURN 0