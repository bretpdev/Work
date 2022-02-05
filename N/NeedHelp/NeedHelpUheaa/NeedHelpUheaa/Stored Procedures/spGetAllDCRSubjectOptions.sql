CREATE PROCEDURE [dbo].[spGetAllDCRSubjectOptions]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DCRSubjectOption as DCRSubjectOptionText,
			AssignToProgrammer
	FROM dbo.LST_DCRSubject
	ORDER BY DCRSubjectOption
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetAllDCRSubjectOptions] TO [db_executor]
    AS [dbo];

