CREATE PROCEDURE [dbo].[spCheckForSubjectBoolean] 
	@Subject				varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
    SELECT AssignToProgrammer FROM dbo.LST_DCRSubject WHERE DCRSubjectOption = @Subject
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCheckForSubjectBoolean] TO [db_executor]
    AS [dbo];

