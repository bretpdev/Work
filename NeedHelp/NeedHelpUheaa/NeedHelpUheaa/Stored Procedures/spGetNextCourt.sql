CREATE PROCEDURE [dbo].[spGetNextCourt]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT SqlUserId, Rotation from dbo.LST_CourtAssignment
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetNextCourt] TO [db_executor]
    AS [dbo];

