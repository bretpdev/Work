CREATE PROCEDURE [schrpt].[GetSchoolRecipients]
	@SchoolId INT = NULL,
	@RecipientId INT = NULL
AS

	SELECT
		sr.SchoolRecipientId, s.SchoolId, s.Name [SchoolName], s.SchoolCode, s.BranchCode,
		r.RecipientId, r.Name [RecipientName], r.Email [RecipientEmail], r.CompanyName [CompanyName],
		rt.ReportTypeId, rt.StoredProcedureName
	FROM
		schrpt.SchoolRecipients sr
		INNER JOIN schrpt.Schools s on s.SchoolId = sr.SchoolId
		INNER JOIN schrpt.Recipients r on r.RecipientId = sr.RecipientId
		INNER JOIN schrpt.ReportTypes rt on rt.ReportTypeId = sr.ReportTypeId
	WHERE
		(sr.SchoolId = @SchoolId OR @SchoolId IS NULL)
		AND
		(sr.RecipientId = @RecipientId OR @RecipientId IS NULL)
		AND
		sr.DeletedAt IS NULL
		AND
		s.DeletedAt IS NULL
		AND
		r.DeletedAt IS NULL
		AND
		rt.DeletedAt IS NULL

RETURN 0
