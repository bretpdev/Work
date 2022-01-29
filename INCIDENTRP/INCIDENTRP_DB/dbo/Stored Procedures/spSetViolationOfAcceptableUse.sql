-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_ViolationOfAcceptableUse table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetViolationOfAcceptableUse]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@AccessKeycardWasShared BIT,
	@MisuseOfSystemResourcesByValidUser BIT,
	@UserSystemCredentialsWereShared BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ViolationOfAcceptableUse WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_ViolationOfAcceptableUse (
				TicketNumber,
				TicketType,
				AccessKeycardWasShared,
				MisuseOfSystemResourcesByValidUser,
				UserSystemCredentialsWereShared
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@AccessKeycardWasShared,
				@MisuseOfSystemResourcesByValidUser,
				@UserSystemCredentialsWereShared
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_ViolationOfAcceptableUse
			SET AccessKeycardWasShared = @AccessKeycardWasShared,
				MisuseOfSystemResourcesByValidUser = @MisuseOfSystemResourcesByValidUser,
				UserSystemCredentialsWereShared = @UserSystemCredentialsWereShared
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END