
create PROCEDURE [dbo].[spAddNewSubjectForDCR]
	@Subject							nvarchar(50),
	@AssignProgrammer					bit
AS
BEGIN

	SET NOCOUNT ON;

		INSERT INTO dbo.LST_DCRSubject
		(DCRSubjectOption, AssignToProgrammer)
		VALUES
		(@Subject, @AssignProgrammer)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spAddNewSubjectForDCR] TO [db_executor]
    AS [dbo];

