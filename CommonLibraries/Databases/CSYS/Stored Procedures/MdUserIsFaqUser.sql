CREATE PROCEDURE MdUserIsFaqUser
	@WindowsUserId nvarchar(10)
AS
    SELECT 1
      FROM SYSA_DAT_Users u
      JOIN SYSA_LST_Role r on r.RoleID = u.[Role]
      JOIN SYSA_DAT_RoleKeyAssignment rka on rka.RoleID = r.RoleID
      JOIN SYSA_LST_UserKeys uk on rka.UserKeyID = uk.ID
     WHERE r.EndDate is null
       AND rka.EndDate is null
       AND u.[Status] = 'Active'
       AND uk.UserKey = 'Tell DUDE'
       AND u.WindowsUserName = @WindowsUserId

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MdUserIsFaqUser] TO [db_executor]
    AS [dbo];

