-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Creates or updates a record in the DAT_Notifier table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetNotifier]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@TicketType VARCHAR(50),
	@NotifierType VARCHAR(15) = NULL,
	@OtherNotifierType VARCHAR(50) = NULL,
	@NotificationMethod VARCHAR(10) = NULL,
	@OtherNotificationMethod VARCHAR(50) = NULL,
	@Name VARCHAR(100) = NULL,
	@EmailAddress VARCHAR(200) = NULL,
	@PhoneNumber VARCHAR(20) = NULL,
	@Relationship VARCHAR(50) = NULL,
	@OtherRelationship VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_Notifier WHERE TicketNumber = @TicketNumber AND TicketType = @TicketType) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_Notifier (
				TicketNumber,
				TicketType,
				[Type],
				OtherType,
				Method,
				OtherMethod,
				[Name],
				EmailAddress,
				PhoneNumber,
				Relationship,
				OtherRelationship
			)
			VALUES (
				@TicketNumber,
				@TicketType,
				@NotifierType,
				@OtherNotifierType,
				@NotificationMethod,
				@OtherNotificationMethod,
				@Name,
				@EmailAddress,
				@PhoneNumber,
				@Relationship,
				@OtherRelationship
			)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_Notifier
			SET [Type] = @NotifierType,
				OtherType = @OtherNotifierType,
				Method = @NotificationMethod,
				OtherMethod = @OtherNotificationMethod,
				[Name] = @Name,
				EmailAddress = @EmailAddress,
				PhoneNumber = @PhoneNumber,
				Relationship = @Relationship,
				OtherRelationship = @OtherRelationship
			WHERE TicketNumber = @TicketNumber
				AND TicketType = @TicketType
		END
END