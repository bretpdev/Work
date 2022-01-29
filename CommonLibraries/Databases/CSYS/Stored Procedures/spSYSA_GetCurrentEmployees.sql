CREATE PROCEDURE [dbo].[spSYSA_GetCurrentEmployees]
AS
BEGIN

	SET NOCOUNT ON;

    SELECT	FirstName+' '+LastName as Name,
			SqlUserId
	FROM	dbo.SYSA_DAT_Users
	WHERE	[Status] = 'Active'
	ORDER BY Name
END
