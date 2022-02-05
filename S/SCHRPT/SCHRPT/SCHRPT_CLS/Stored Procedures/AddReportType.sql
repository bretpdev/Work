CREATE PROCEDURE [schrpt].[AddReportType]
	@StoredProcedureName VARCHAR(50),
	@WindowsUserName VARCHAR(50)
AS

	INSERT INTO schrpt.ReportTypes(StoredProcedureName, AddedBy)
	VALUES (@StoredProcedureName, @WindowsUserName)

	SELECT CAST(SCOPE_IDENTITY() AS INT)

RETURN 0
