create PROCEDURE [dbo].[spUpdateSubjectBoolean] 
	@Subject								VARCHAR(50),
	@isProgrammer							bit
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE dbo.LST_DCRSubject
	SET AssignToProgrammer = @isProgrammer
	WHERE DCRSubjectOption = @Subject

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spUpdateSubjectBoolean] TO [db_executor]
    AS [dbo];

