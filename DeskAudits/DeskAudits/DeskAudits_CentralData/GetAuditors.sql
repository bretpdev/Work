CREATE PROCEDURE [deskaudits].[GetAuditors]

AS

	SELECT DISTINCT
		Auditor
	FROM
		deskaudits.Audits
	WHERE
		DeletedAt IS NULL
	ORDER BY
		Auditor ASC

RETURN 0
