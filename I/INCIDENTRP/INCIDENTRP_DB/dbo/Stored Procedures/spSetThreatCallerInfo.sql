-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Creates or updates a record in the DAT_ThreatCallerInfo table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetThreatCallerInfo]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@CallDuration VARCHAR(50) = NULL,
	@RecognizedTheCallersVoice BIT = 0,
	@CallerIsFamiliarWithUheaa BIT = 0,
	@FamiliaritySpecifics TEXT = NULL,
	@Sex VARCHAR(10) = NULL,
	@Age VARCHAR(10) = NULL,
	@Name VARCHAR(50) = NULL,
	@PhoneNumber VARCHAR(20) = NULL,
	@Address VARCHAR(100) = NULL,
	@AccountNumber VARCHAR(10) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ThreatCallerInfo WHERE TicketNumber = @TicketNumber) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_ThreatCallerInfo (TicketNumber, TicketType, CallDuration, RecognizedTheCallersVoice, CallerIsFamiliarWithUheaa, FamiliaritySpecifics, Sex, Age, [Name], PhoneNumber, [Address], AccountNumber)
			VALUES (@TicketNumber, 'Threat', @CallDuration, @RecognizedTheCallersVoice, @CallerIsFamiliarWithUheaa, @FamiliaritySpecifics, @Sex, @Age, @Name, @PhoneNumber, @Address, @AccountNumber)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_ThreatCallerInfo
			SET CallDuration = @CallDuration,
				RecognizedTheCallersVoice = @RecognizedTheCallersVoice,
				CallerIsFamiliarWithUheaa = @CallerIsFamiliarWithUheaa,
				FamiliaritySpecifics = @FamiliaritySpecifics,
				Sex = @Sex,
				Age = @Age,
				[Name] = @Name,
				PhoneNumber = @PhoneNumber,
				[Address] = @Address,
				AccountNumber = @AccountNumber
			WHERE TicketNumber = @TicketNumber
		END
END