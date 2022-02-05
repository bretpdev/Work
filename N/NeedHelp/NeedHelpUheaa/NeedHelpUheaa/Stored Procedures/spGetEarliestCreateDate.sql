CREATE PROCEDURE [dbo].[spGetEarliestCreateDate]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT COALESCE(MIN(Requested), GETDATE())
    from DAT_Ticket
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetEarliestCreateDate] TO [db_executor]
    AS [dbo];

