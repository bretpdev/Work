CREATE PROCEDURE [dbo].[spGetListOfSystems]

AS
BEGIN
	SET NOCOUNT ON;

    SELECT [System]
	FROM dbo.LST_System
	WHERE ValidForTicketType = 'GEN'
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetListOfSystems] TO [db_executor]
    AS [dbo];

