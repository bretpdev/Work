CREATE PROCEDURE [dbo].[spNDHP_GetManagerEmail]
	@BusinessUnit	int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		WindowsUserName
	FROM
		SYSA_DAT_Users
	WHERE
		Title = 'Manager'
		AND BusinessUnit = @BusinessUnit
		AND [Status] = 'Active'
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spNDHP_GetManagerEmail] TO [db_executor]
    AS [dbo];

