



CREATE PROCEDURE [dbo].[spNDHP_SaveTicketUpdate] 

@Ticket 	bigint,
@Role		nvarchar(50),
@ChangeTo	nvarchar(50)

AS

If @ChangeTo = ''OR @ChangeTo is NULL
	BEGIN
		IF (SELECT UserID FROM dbo.NDHP_DAT_UpdateTicketUserIDs WHERE Ticket = @Ticket AND Role = @Role) <> ''
			BEGIN --Record exists. Need to delete it.
			/*Print 'Deleted Existing Record 1.1'*/
				DELETE FROM dbo.NDHP_DAT_UpdateTicketUserIDs 
				WHERE 	(Ticket = @Ticket) AND (Role = @Role)

			END
		Else IF (SELECT Identifier FROM dbo.NDHP_DAT_UpdateTicketUserIDs WHERE Ticket = @Ticket AND Role = @Role) > 0 
			--Record exists but is Null or blank. Really shouldn't ever happen once data is in correctly.
			Begin
			/*Print 'Deleted Blank Record'*/
				DELETE FROM dbo.NDHP_DAT_UpdateTicketUserIDs 
				WHERE 	(Ticket = @Ticket) AND (Role = @Role)
			End

		/*Else --Record does not exist. Nothing needs to be done.
			Begin
			Print 'Do Nothing 1.3'
		End*/
	END

Else -- Value to be updated is not blank
		BEGIN
		IF (SELECT UserID FROM dbo.NDHP_DAT_UpdateTicketUserIDs WHERE Ticket = @Ticket AND Role = @Role) <> ''
			BEGIN --Record exists already just update it.
			/*Print 'Updated Record 2.1'*/
				UPDATE dbo.NDHP_DAT_UpdateTicketUserIDs 
				SET 	UserID = @ChangeTo
				WHERE 	(Ticket = @Ticket) AND (Role = @Role)
			END
		Else IF (SELECT Identifier FROM dbo.NDHP_DAT_UpdateTicketUserIDs WHERE Ticket = @Ticket AND Role = @Role) > 0 
			--Record exists but is Null or blank. Really shouldn't ever happen once data is in correctly.
			Begin
			/*Print 'Updated Blank Record 2.2'*/
				UPDATE dbo.NDHP_DAT_UpdateTicketUserIDs 
				SET 	UserID = @ChangeTo
				WHERE 	(Ticket = @Ticket) AND (Role = @Role)
			End

		Else --Record does not exist and needs to be created
			Begin
			/*Print 'Inserted Record 2.3'*/
				INSERT INTO dbo.NDHP_DAT_UpdateTicketUserIDs 
				VALUES (@Ticket, @Role, @ChangeTo)
		End
	END