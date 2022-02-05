CREATE PROCEDURE [dbo].[GetRunningTicket] 
	@TicketID int = 0, 
	@SqlUserID int = 0,
	@Region varchar(11)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		StartTime
	FROM
		TimeTracking
	WHERE
		SqlUserID = @SqlUserID
		AND TicketID = @TicketID
		AND EndTime IS NULL
		AND Region = @Region
	
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRunningTicket] TO [db_executor]
    AS [dbo];