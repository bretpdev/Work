create PROCEDURE [dbo].[spCheckForNewSubject] 
	@Subject				varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
    SELECT DCRSubjectOption FROM dbo.LST_DCRSubject WHERE DCRSubjectOption = @Subject
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCheckForNewSubject] TO [db_executor]
    AS [dbo];

