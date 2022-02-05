CREATE PROCEDURE [dbo].[UpdateSchedule]
	@StatusScheduleId INT,
	@Start DATETIME,
	@End DATETIME,
	@StatusCodeId INT,
	@RegionId INT,
	@UpdatedBy VARCHAR(50)
AS
	
	UPDATE
		StatusSchedules
	SET
		StartAt = @Start,
		EndAt = @End,
		StatusCodeId = @StatusCodeId,
		RegionId = @RegionId,
		UpdatedBy = @UpdatedBy,
		UpdatedAt = GETDATE()
	WHERE
		StatusScheduleId = @StatusScheduleId
RETURN 0