
CREATE PROCEDURE [dbo].[spSYSA_GetApplicationKeys]
	@Application VARCHAR(100) = ''
AS
BEGIN
	SET NOCOUNT ON;

	IF @Application <> ''
		SELECT ID,
			[Application],
			UserKey AS Name,
			[Type],
			[Description]
		FROM SYSA_LST_UserKeys
		WHERE [Application] = @Application
		AND EndDate IS NULL
		ORDER BY Name
	ELSE
		SELECT ID,
			[Application],
			UserKey AS Name,
			[Type],
			[Description]
		FROM SYSA_LST_UserKeys
		WHERE EndDate IS NULL
		ORDER BY Name
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetApplicationKeys] TO [db_executor]
    AS [dbo];

