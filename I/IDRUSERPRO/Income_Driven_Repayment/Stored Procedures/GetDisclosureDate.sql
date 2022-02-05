CREATE PROCEDURE [dbo].[GetDisclosureDate]
	@AppId INT
AS

	SELECT
		updated_at
	FROM
		Applications
	WHERE
		application_id = @AppId

RETURN 0