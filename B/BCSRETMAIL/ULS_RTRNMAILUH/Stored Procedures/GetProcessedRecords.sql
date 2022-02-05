CREATE PROCEDURE [rtrnmailuh].[GetProcessedRecords]
	@SelectedDate DATETIME
AS
	SELECT
		BD.AccountIdentifier,
		BD.LetterId,
		'Compass' [System],
		BD.CreateDate,
		BD.ReceivedDate,
		CASE
			WHEN BD.Address1 != '' OR BD.Address2 != '' OR BD.City != '' OR BD.[State] != '' OR BD.Zip != '' THEN 1
			ELSE 0
		END HasForwarding,
		REPLACE(BD.AddedBy, 'UHEAA\', '') [AddedBy],
		BD.AddedAt,
		BD.ProcessedAt,
		BD.ArcAddProcessingId,
		AA.ProcessedAt [ArcProcessedAt]
	FROM
		ULS.rtrnmailuh.BarcodeData BD
		LEFT JOIN ULS..ArcAddProcessing AA
			ON AA.ArcAddProcessingId = BD.ArcAddProcessingId
	WHERE
		CAST(BD.AddedAt AS DATE) = CAST(@SelectedDate AS DATE)
		AND BD.DeletedAt IS NULL

RETURN 0