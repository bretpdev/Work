CREATE PROCEDURE dbo.spQCTR_LockAndUnlock 

@LockOrUnlock		varchar(6) ,
@User				varchar(50),
@ID			bigint

AS

IF (@LockOrUnlock = 'Lock')
	BEGIN
		IF ((SELECT COUNT(*)
		    FROM dbo.QCTR_DAT_InUse
		    WHERE IssueID = @ID) > 0)
			BEGIN
				/*check if the ticket is locked by the user*/
				IF ((SELECT COUNT(*)
		    		FROM dbo.QCTR_DAT_InUse
		    		WHERE IssueID = @ID AND UserID <> @User) > 0)
					BEGIN
						/*ticket locked by another user so the user can't be allowed to access it*/
						SELECT 'Ticket is locked by ' + B.FirstName + ' ' + B.LastName + '.  Please try again later.' as Response
						FROM  dbo.QCTR_DAT_InUse A
						JOIN dbo.SYSA_LST_Users B ON A.UserID = B.WindowsUserName
						WHERE A.IssueID = @ID
					END
			END
		ELSE
			BEGIN
				DELETE FROM dbo.QCTR_DAT_InUse WHERE UserID = @User /*unlock tickets user currently has locked*/
				INSERT INTO dbo.QCTR_DAT_InUse (IssueID, UserID) VALUES (@ID, @User) /*lock ticket*/  
			END
	END
ELSE
	BEGIN
		DELETE FROM dbo.QCTR_DAT_InUse WHERE UserID = @User /*unlock ticket*/
	END