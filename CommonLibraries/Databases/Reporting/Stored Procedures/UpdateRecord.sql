CREATE PROCEDURE [dbo].[UpdateRecord]
	@TimeTrackingId int,
	@StartTime datetime,
	@EndTime datetime,
	@SqlUserId int,
	@SystemTypeId int,
	@CostCenterId int = null,
	@BatchProcessing bit,
	@GenericMeeting varchar(250) = null,
	@Region varchar(11)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE
		TimeTracking
	SET
		StartTime = @StartTime,
		EndTime = @EndTime,
		SystemTypeId = @SystemTypeId,
		CostCenterId = @CostCenterId,
		BatchProcessing = @BatchProcessing,
		GenericMeeting = @GenericMeeting,
		Region = @Region
	WHERE
		TimeTrackingId = @TimeTrackingId
		
	SELECT @@ROWCOUNT
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateRecord] TO [db_executor]
    AS [dbo];