CREATE PROCEDURE [dbo].[SetStartTime] 
	-- Add the parameters for the stored procedure here
	@SqlUserID int = 0,
	@TicketID int = null,
	@Region varchar(11),
	@StartTime datetime,
	@CostCenterId int = null,
	@SystemTypeId int = null,
	@BatchProcessing bit = 0,
	@GenericMeeting varchar(250) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE
		TimeTracking
	SET
		EndTime = GETDATE()
	WHERE
		EndTime IS NULL
		AND SqlUserID = @SqlUserID

    -- Insert statements for procedure here
	INSERT INTO TimeTracking(SqlUserID, TicketID, StartTime, Region, CostCenterId, SystemTypeId, BatchProcessing, GenericMeeting)
	VALUES(@SqlUserID, @TicketID, @StartTime, @Region, @CostCenterId, @SystemTypeId, @BatchProcessing, @GenericMeeting)
	
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetStartTime] TO [db_executor]
    AS [dbo];