CREATE PROCEDURE [dbo].[SetStartTime]
	@SqlUserID int = 0,
	@TicketID int = null,
	@StartTime datetime,
	@CostCenterId int = null,
	@SystemTypeId int = null,
	@BatchProcessing bit = 0,
	@GenericMeeting varchar(250) = null
AS
	UPDATE
		TimeTracking
	SET
		EndTime = GETDATE()
	WHERE
		EndTime IS NULL
		AND SqlUserID = @SqlUserID

	INSERT INTO TimeTracking(SqlUserID, TicketID, StartTime, CostCenterId, SystemTypeId, BatchProcessing, GenericMeeting)
	VALUES(@SqlUserID, @TicketID, @StartTime, @CostCenterId, @SystemTypeId, @BatchProcessing, @GenericMeeting)