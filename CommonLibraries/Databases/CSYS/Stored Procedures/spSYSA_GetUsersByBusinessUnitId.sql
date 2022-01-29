CREATE PROCEDURE [dbo].[spSYSA_GetUsersByBusinessUnitId]
	@BusinessUnitId int
AS
	select u.SqlUserId, u.WindowsUserName, u.FirstName, u.LastName
	  from SYSA_DAT_Users u
	 where u.BusinessUnit = @BusinessUnitId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetUsersByBusinessUnitId] TO [db_executor]
    AS [dbo];

