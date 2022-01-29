CREATE PROCEDURE [dbo].[spGetCommercialSubject]

AS
BEGIN
	SET NOCOUNT ON;

    SELECT Distinct [Subject] from dbo.DAT_Ticket
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetCommercialSubject] TO [db_executor]
    AS [dbo];

