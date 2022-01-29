
CREATE PROCEDURE spQSTA_GetDepartments

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DeptCode FROM dbo.QSTA_LST_Departments
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spQSTA_GetDepartments] TO [db_executor]
    AS [dbo];

