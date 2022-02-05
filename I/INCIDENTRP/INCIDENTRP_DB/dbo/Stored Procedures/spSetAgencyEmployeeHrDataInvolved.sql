-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_AgencyEmployeeHrDataInvolved table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetAgencyEmployeeHrDataInvolved]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@Name VARCHAR(100) = NULL,
	@State CHAR(2) = NULL,
	@NotifierKnowsEmployee BIT,
	@NotifierRelationshipToEmployee VARCHAR(50) = NULL,
	@DateOfBirthWasReleased BIT,
	@EmployeeIdNumberWasReleased BIT,
	@HomeAddressWasReleased BIT,
	@HealthInformationWasReleased BIT,
	@PerformanceInformationWasReleased BIT,
	@PersonnelFilesWereReleased BIT,
	@UnauthorizedReferenceWasReleased BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_AgencyEmployeeHrDataInvolved WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_AgencyEmployeeHrDataInvolved (
				TicketNumber,
				TicketType,
				[Name],
				[State],
				NotifierKnowsEmployee,
				NotifierRelationshipToEmployee,
				DateOfBirthWasReleased,
				EmployeeIdNumberWasReleased,
				HomeAddressWasReleased,
				HealthInformationWasReleased,
				PerformanceInformationWasReleased,
				PersonnelFilesWereReleased,
				UnauthorizedReferenceWasReleased
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@Name,
				@State,
				@NotifierKnowsEmployee,
				@NotifierRelationshipToEmployee,
				@DateOfBirthWasReleased,
				@EmployeeIdNumberWasReleased,
				@HomeAddressWasReleased,
				@HealthInformationWasReleased,
				@PerformanceInformationWasReleased,
				@PersonnelFilesWereReleased,
				@UnauthorizedReferenceWasReleased
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_AgencyEmployeeHrDataInvolved
			SET [Name] = @Name,
				[State] = @State,
				NotifierKnowsEmployee = @NotifierKnowsEmployee,
				NotifierRelationshipToEmployee = @NotifierRelationshipToEmployee,
				DateOfBirthWasReleased = @DateOfBirthWasReleased,
				EmployeeIdNumberWasReleased = @EmployeeIdNumberWasReleased,
				HomeAddressWasReleased = @HomeAddressWasReleased,
				HealthInformationWasReleased = @HealthInformationWasReleased,
				PerformanceInformationWasReleased = @PerformanceInformationWasReleased,
				PersonnelFilesWereReleased = @PersonnelFilesWereReleased,
				UnauthorizedReferenceWasReleased = @UnauthorizedReferenceWasReleased
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END