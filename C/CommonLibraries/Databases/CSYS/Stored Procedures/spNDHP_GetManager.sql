CREATE PROCEDURE [dbo].[spNDHP_GetManager]
	@BusinessUnit	int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		FirstName + ' ' + LastName
	FROM
		SYSA_DAT_Users
	WHERE
		Title = 'Manager'
		AND BusinessUnit = @BusinessUnit
		AND [Status] = 'Active'
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spNDHP_GetManager] TO [db_executor]
    AS [dbo];

