CREATE PROCEDURE [dbo].[UpdateExistingRecord]
	@MetricSummaryId INT,
	@TotalRecords INT,
	@ComplaintRecords INT,
	@AvgBacklog INT
AS
	UPDATE
		MetricsSummary
	SET
		CompliantRecords = @ComplaintRecords,
		TotalRecords = @TotalRecords,
		AverageBacklogAge = @AvgBacklog,
		UpdatedAt = GETDATE(),
		UpdatedBy = SYSTEM_USER
	WHERE
		MetricsSummaryId = @MetricSummaryId
RETURN 0
