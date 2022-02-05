CREATE PROCEDURE [dbo].[spSYSA_UserHasUserKey]
	@WindowsUserName nvarchar(50),
	@UserKey nvarchar(50),
	@RequireBusinessUnit bit = 0
AS
if EXISTS(
	select 1
	  from SYSA_DAT_Users u
	  join SYSA_DAT_UserKeyAssignment uka on uka.SqlUserId = u.SqlUserId
	 where uka.EndDate is null
	   and u.[Status] = 'Active'
	   and uka.UserKey = @UserKey
	   and u.WindowsUserName = @WindowsUserName
	   and (@RequireBusinessUnit = 0 or u.BusinessUnit = uka.BusinessUnit)
) select cast(1 as bit) else select cast(0 as bit)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_UserHasUserKey] TO [db_executor]
    AS [dbo];

