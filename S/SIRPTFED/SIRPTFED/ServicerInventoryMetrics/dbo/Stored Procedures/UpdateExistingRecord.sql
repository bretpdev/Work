CREATE PROCEDURE [dbo].[UpdateExistingRecord]
	@MetricSummaryId INT,
	@TotalRecords INT,
	@ComplaintRecords INT,
	@AvgBacklog INT,
	@SuspenseAmount DECIMAL(14,2) NULL,
	@SuspenseTotal DECIMAL(14,2) NULL
AS
	UPDATE
		MetricsSummary
	SET
		CompliantRecords = @ComplaintRecords,
		TotalRecords = @TotalRecords,
		AverageBacklogAge = @AvgBacklog,
		SuspenseAmount = @SuspenseAmount,
		SuspenseTotal = @SuspenseTotal,
		UpdatedAt = GETDATE(),
		UpdatedBy = SYSTEM_USER
	WHERE
		MetricsSummaryId = @MetricSummaryId
RETURN 0