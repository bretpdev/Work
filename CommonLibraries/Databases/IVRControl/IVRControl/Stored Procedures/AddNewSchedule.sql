CREATE PROCEDURE [dbo].[AddNewSchedule]
	@Start DATETIME,
	@End DATETIME,
	@StatusCodeId INT,
	@RegionId INT,
	@AddedBy VARCHAR(50)
AS
	INSERT INTO StatusSchedules(StartAt, EndAt, StatusCodeId, RegionId, AddedBy)
	VALUES(@Start, @End, @StatusCodeId, @RegionId, @AddedBy)
RETURN 0
