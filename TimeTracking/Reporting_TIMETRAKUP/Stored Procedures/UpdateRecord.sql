CREATE PROCEDURE [dbo].[UpdateRecord]
	@TimeTrackingId int,
	@StartTime datetime,
	@EndTime datetime,
	@SqlUserId int,
	@SystemTypeId int,
	@CostCenterId int = null,
	@BatchProcessing bit,
	@GenericMeeting varchar(250) = null
AS
	UPDATE
		TimeTracking
	SET
		StartTime = @StartTime,
		EndTime = @EndTime,
		SystemTypeId = @SystemTypeId,
		CostCenterId = @CostCenterId,
		BatchProcessing = @BatchProcessing,
		GenericMeeting = @GenericMeeting,
		Region = 'uheaa'
	WHERE
		TimeTrackingId = @TimeTrackingId
		
	SELECT @@ROWCOUNT