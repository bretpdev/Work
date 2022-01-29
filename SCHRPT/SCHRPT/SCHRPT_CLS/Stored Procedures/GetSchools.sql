CREATE PROCEDURE [schrpt].[GetSchools]
AS

	SELECT
		s.SchoolId, s.SchoolCode, s.BranchCode, s.Name, COUNT(sr.SchoolRecipientId) [RecipientCount]
	FROM
		schrpt.Schools s
		LEFT JOIN schrpt.SchoolRecipients sr ON s.SchoolId = sr.SchoolId AND sr.DeletedAt IS NULL
	WHERE
		s.DeletedAt IS NULL
	GROUP BY
		s.SchoolId, s.SchoolCode, s.BranchCode, s.Name
	ORDER BY
		s.Name

RETURN 0
