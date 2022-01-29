CREATE PROCEDURE [schrpt].[GetReportTypes]
AS

	SELECT
		rt.ReportTypeId, rt.StoredProcedureName
	FROM
		schrpt.ReportTypes rt
	WHERE
		rt.DeletedAt IS NULL

RETURN 0