CREATE PROCEDURE [dbo].[spGetSubjectList]

AS
BEGIN
	SET NOCOUNT ON;

    SELECT DCRSubjectOption from dbo.LST_DCRSubject
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetSubjectList] TO [db_executor]
    AS [dbo];

