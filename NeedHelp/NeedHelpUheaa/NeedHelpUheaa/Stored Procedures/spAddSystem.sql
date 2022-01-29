
CREATE PROCEDURE [dbo].[spAddSystem]
	@TicketID			bigint,
	@System				varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO dbo.REF_System (Ticket, [System]) VALUES (@TicketID, @System)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spAddSystem] TO [db_executor]
    AS [dbo];

