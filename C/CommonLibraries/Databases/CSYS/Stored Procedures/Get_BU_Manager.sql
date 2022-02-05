CREATE PROCEDURE [dbo].[Get_BU_Manager]
	@BusinessUnitName varchar(25) = '', 
	@BusinessUnitId int = 0

AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		SqlUserId,
		WindowsUserName,
		FirstName,
		MiddleInitial,
		LastName,
		EMail,
		Extension,
		Extension2,
		AesAccountId,
		Title,
		BusinessUnit,
		[Role],
		AesUserId
	FROM
		SYSA_DAT_Users
	WHERE
		BusinessUnit LIKE
		CASE WHEN @BusinessUnitName <> '' THEN
			(SELECT ID FROM GENR_LST_BusinessUnits WHERE Name = @BusinessUnitName)
		ELSE
			@BusinessUnitId
		END
		AND Title = 'Manager'
		AND [Status] = 'Active'

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Get_BU_Manager] TO [db_executor]
    AS [dbo];

