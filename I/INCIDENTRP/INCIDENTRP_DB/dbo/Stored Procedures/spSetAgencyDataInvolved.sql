-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_AgencyDataInvolved table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetAgencyDataInvolved]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@AccountingOrAdministrativeRecordsWereReleased BIT,
	@ClosedSchoolRecordsWereReleased BIT,
	@ConfidentialCaseFilesWereReleased BIT,
	@ContractInformationWasReleased BIT,
	@OperationsReportsWereReleased BIT,
	@ProposalAndLoanPurchaseRequestsWereReleased BIT,
	@UespParticipantRecordsWereReleased BIT,
	@OtherInformationWasReleased BIT,
	@OtherInformation VARCHAR(100) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_AgencyDataInvolved WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_AgencyDataInvolved (
				TicketNumber,
				TicketType,
				AccountingOrAdministrativeRecordsWereReleased,
				ClosedSchoolRecordsWereReleased,
				ConfidentialCaseFilesWereReleased,
				ContractInformationWasReleased,
				OperationsReportsWereReleased,
				ProposalAndLoanPurchaseRequestsWereReleased,
				UespParticipantRecordsWereReleased,
				OtherInformationWasReleased,
				OtherInformation
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@AccountingOrAdministrativeRecordsWereReleased,
				@ClosedSchoolRecordsWereReleased,
				@ConfidentialCaseFilesWereReleased,
				@ContractInformationWasReleased,
				@OperationsReportsWereReleased,
				@ProposalAndLoanPurchaseRequestsWereReleased,
				@UespParticipantRecordsWereReleased,
				@OtherInformationWasReleased,
				@OtherInformation
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_AgencyDataInvolved
			SET AccountingOrAdministrativeRecordsWereReleased = @AccountingOrAdministrativeRecordsWereReleased,
				ClosedSchoolRecordsWereReleased = @ClosedSchoolRecordsWereReleased,
				ConfidentialCaseFilesWereReleased = @ConfidentialCaseFilesWereReleased,
				ContractInformationWasReleased = @ContractInformationWasReleased,
				OperationsReportsWereReleased = @OperationsReportsWereReleased,
				ProposalAndLoanPurchaseRequestsWereReleased = @ProposalAndLoanPurchaseRequestsWereReleased,
				UespParticipantRecordsWereReleased = @UespParticipantRecordsWereReleased,
				OtherInformationWasReleased = @OtherInformationWasReleased,
				OtherInformation = @OtherInformation
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END