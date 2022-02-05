
CREATE PROCEDURE [scra].[SetTXCXIndicators]
	@Success BIT,
	@ActiveDutyReportingId INT
AS

IF @Success = 1
BEGIN
	UPDATE
		[scra].ActiveDutyReporting
	SET
		TXCXUpdated = GETDATE()
	WHERE
		ActiveDutyReportingId = @ActiveDutyReportingId
		AND TXCXUpdated IS NULL
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL
		AND ErroredAt IS NULL
END
ELSE
BEGIN
	UPDATE
		[scra].ActiveDutyReporting
	SET
		ErroredAt = GETDATE()
	WHERE
		ActiveDutyReportingId = @ActiveDutyReportingId
		AND TXCXUpdated IS NULL
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL
		AND ErroredAt IS NULL
END