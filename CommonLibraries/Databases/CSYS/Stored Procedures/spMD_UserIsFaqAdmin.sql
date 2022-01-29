CREATE PROCEDURE spMD_UserIsFaqAdmin
	@WindowsUserName nvarchar(50)
AS
if EXISTS(
	select 1
	  from SYSA_DAT_Users u
	  join SYSA_LST_Role r on r.RoleID = u.Role
	  join SYSA_DAT_RoleKeyAssignment rka on rka.RoleID = r.RoleID
	  join SYSA_LST_UserKeys uk on rka.UserKeyID = uk.ID
	 where r.EndDate is null
	   and rka.EndDate is null
	   and u.[Status] = 'Active'
	   and uk.UserKey = 'Tell DUDE'
	   and u.WindowsUserName = @WindowsUserName
) select cast(1 as bit) else select cast(0 as bit)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_UserIsFaqAdmin] TO [db_executor]
    AS [dbo];

