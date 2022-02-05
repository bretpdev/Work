CREATE PROCEDURE [deskaudits].[GetAuditees]

AS

	SELECT DISTINCT
		Auditee
	FROM
		deskaudits.Audits
	WHERE
		DeletedAt IS NULL
	ORDER BY
		Auditee ASC

RETURN 0
