-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Creates or updates a record in the DAT_ThreatBackgroundNoise table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetThreatBackgroundNoise]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@Airplanes BIT = 0,
	@Animals BIT = 0,
	@Conversation BIT = 0,
	@Crowd BIT = 0,
	@FactoryMachines BIT = 0,
	@Music BIT = 0,
	@OfficeMachines BIT = 0,
	@Party BIT = 0,
	@PublicAddressSystem BIT = 0,
	@SchoolBell BIT = 0,
	@StreetTraffic BIT = 0,
	@Trains BIT = 0,
	@Other BIT = 0,
	@OtherDescription VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ThreatBackgroundNoise WHERE TicketNumber = @TicketNumber) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_ThreatBackgroundNoise (TicketNumber, TicketType, Airplanes, Animals, [Conversation], Crowd, FactoryMachines, Music, OfficeMachines, Party, PublicAddressSystem, SchoolBell, StreetTraffic, Trains, Other, OtherDescription)
			VALUES (@TicketNumber, 'Threat', @Airplanes, @Animals, @Conversation, @Crowd, @FactoryMachines, @Music, @OfficeMachines, @Party, @PublicAddressSystem, @SchoolBell, @StreetTraffic, @Trains, @Other, @OtherDescription)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_ThreatBackgroundNoise
			SET Airplanes = @Airplanes,
				Animals = @Animals,
				[Conversation] = @Conversation,
				Crowd = @Crowd,
				FactoryMachines = @FactoryMachines,
				Music = @Music,
				OfficeMachines = @OfficeMachines,
				Party = @Party,
				PublicAddressSystem = @PublicAddressSystem,
				SchoolBell = @SchoolBell,
				StreetTraffic = @StreetTraffic,
				Trains = @Trains,
				Other = @Other,
				OtherDescription = @OtherDescription
			WHERE TicketNumber = @TicketNumber
		END
END