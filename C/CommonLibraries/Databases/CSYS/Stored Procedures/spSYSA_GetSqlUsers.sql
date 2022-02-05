CREATE PROCEDURE [dbo].[spSYSA_GetSqlUsers]
	@IncludeInactiveRecords		BIT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		SqlUserId AS ID,
		WindowsUserName,
		FirstName,
		MiddleInitial,
		LastName,
		EMail AS EmailAddress,
		Extension AS PrimaryExtension,
		Extension2 AS SecondaryExtension,
		AesAccountId AS AesAccountNumber,
		AesUserId,
		[Title],
		BusinessUnit,
		[Role],
		[Status]
	FROM SYSA_DAT_Users
	WHERE (@IncludeInactiveRecords = 1 OR [Status] = 'Active')
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetSqlUsers] TO [db_executor]
    AS [dbo];

