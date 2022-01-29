CREATE PROCEDURE [schrpt].[GetRecipients]
AS

	SELECT
		r.RecipientId, r.Name, r.Email, r.CompanyName, COUNT(sr.RecipientId) [SchoolCount]
	FROM
		schrpt.Recipients r
		LEFT JOIN
		(
			SELECT
				sr.RecipientId
			FROM
				schrpt.SchoolRecipients sr 
				INNER JOIN schrpt.Schools s ON sr.SchoolId = s.SchoolId AND s.DeletedAt IS NULL
			WHERE
				sr.DeletedAt IS NULL
				AND
				s.DeletedAt IS NULL
		) sr ON sr.RecipientId = r.RecipientId
	WHERE
		r.DeletedAt IS NULL
	GROUP BY
		r.RecipientId, r.Name, r.Email, r.CompanyName
	ORDER BY
		r.Name

RETURN 0
