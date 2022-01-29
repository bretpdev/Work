CREATE PROCEDURE [schrpt].[GetSchool]
	@SchoolId INT
AS

	SELECT
		s.SchoolId, s.Name, s.SchoolCode, s.BranchCode, COUNT(sr.SchoolRecipientId) [RecipientCount]
	FROM
		schrpt.Schools s
		LEFT JOIN schrpt.SchoolRecipients sr ON sr.SchoolId = s.SchoolId AND sr.DeletedAt IS NULL
	WHERE
		s.SchoolId = @SchoolId
	GROUP BY
		s.SchoolId, s.Name, s.SchoolCode, s.BranchCode

RETURN 0
