CREATE PROCEDURE [rtnemlinvf].[GetEmailAddresses]
AS
	SELECT
		EmailAddress
	FROM
		EmailAddresses
	WHERE
		DeletedAt IS NULL
RETURN 0