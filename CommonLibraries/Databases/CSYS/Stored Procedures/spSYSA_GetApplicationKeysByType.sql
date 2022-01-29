
CREATE PROCEDURE [dbo].[spSYSA_GetApplicationKeysByType]
	@Application VARCHAR(100) = ''
	, @Type varchar(20)
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
		AND [Type] = @Type
		AND EndDate IS NULL
		ORDER BY Name
	ELSE
		SELECT ID,
			[Application],
			UserKey AS Name,
			[Type],
			[Description]
		FROM SYSA_LST_UserKeys
		WHERE [Type] = @Type
		AND EndDate IS NULL
		ORDER BY Name
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetApplicationKeysByType] TO [db_executor]
    AS [dbo];

