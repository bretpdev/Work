CREATE PROCEDURE [dbo].[spGetResolutionCauses]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT Cause from dbo.LST_ResolutionCause
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetResolutionCauses] TO [db_executor]
    AS [dbo];

