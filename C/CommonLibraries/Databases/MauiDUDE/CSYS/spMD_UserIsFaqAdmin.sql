USE [CSYS]
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('spMD_UserIsFaqAdmin') AND TYPE IN ('P', 'PC'))
DROP PROCEDURE spMD_UserIsFaqAdmin
GO
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

GRANT EXECUTE ON spMD_UserIsFaqAdmin TO db_executor
GO
