CREATE PROCEDURE dbo.spNDHP_LockAndUnlock 

@LockOrUnlock		varchar(6) ,
@User				varchar(50),
@Ticket			bigint

AS

IF (@LockOrUnlock = 'Lock')
	BEGIN
		IF ((SELECT COUNT(*)
		    FROM dbo.NDHP_LST_InUse
		    WHERE Ticket = @Ticket) > 0)
			BEGIN
				/*check if the ticket is locked by the user*/
				IF ((SELECT COUNT(*)
		    		FROM dbo.NDHP_LST_InUse
		    		WHERE Ticket = @Ticket AND UserName <> @User) > 0)
					BEGIN
						/*ticket locked by another user so the user can't be allowed to access it*/
						SELECT 'Ticket is locked by ' + B.FirstName + ' ' + B.LastName + '.  Please try again later.' as Response
						FROM  dbo.NDHP_LST_InUse A
						JOIN dbo.SYSA_LST_Users B ON A.UserName = B.WindowsUserName
						WHERE A.Ticket = @Ticket
					END
			END
		ELSE
			BEGIN
				DELETE FROM dbo.NDHP_LST_InUse WHERE UserName = @User /*unlock tickets user currently has locked*/
				INSERT INTO dbo.NDHP_LST_InUse (Ticket, UserName) VALUES (@Ticket, @User) /*lock ticket*/  
			END
	END
ELSE
	BEGIN
		DELETE FROM dbo.NDHP_LST_InUse WHERE UserName = @User /*unlock ticket*/
	END