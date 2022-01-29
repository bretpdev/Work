-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/04/2013
-- Description:	WILL UPDATE THE APPLICATIONS TABLE WITH CHANGES MADE BY THE USER
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateApp] 

@ApplicationId INT,
@EApplicationId CHAR(10) = NULL,
@AwardId VARCHAR(50) = NULL,
@SpouseId INT = NULL,
@ApplicationSourceId INT,
@RepaymentPlanStatusId INT = NULL,
@DisclosureDate DATE = NULL,
@RepaymentPlanDateEntered DATE = NULL,
@FilingStatusId INT = NULL,
@FamilySize INT = NULL,
@TaxYear INT = NULL,
@LoansAtOtherServicers BIT = NULL,
@JointRepaymentPlan BIT = NULL,
@Agi MONEY = NULL,
@AgiReflectIncome BIT = NULL,
@ManuallySubmittedIncomeIndicator BIT = NULL,
@ManuallySubmittedIncome MONEY = NULL,
@SupportingDocumentationRequired BIT = NULL,
@SupportingDocumentationRecDate DATE = NULL,
@RequestedByBorrower BIT = NULL,
@BorrowerEligibilityId INT = NULL,
@IncomeSourceId INT = NULL,
@RepaymentPlanReason INT = NULL,
@RepaymentPlanTypeRequested INT = NULL,
@DueDateRequested char(2) = NULL,
@State char(2) = NULL,
@ReceivedDate DATE = NULL,
@TaxesFiledFlag bit = null,
@BorrowerSelectedLowest bit = null,
@UpdatedBy VARCHAR(50),
@Active bit,
@RepaymentTypeProcessedNotSame bit = null,
@NumberOfChildren int = null,
@NumberOfDependents int = null,
@TotalIncome decimal = null,
@DefForbId int = null,
@PublicServiceEmployment varchar(200) = null,
@RPF decimal = null,
@MartialStatusId int = null,
@GradeLevel char(1) = null,
@IncludeSpouseInFamilySize bit = NULL

AS
BEGIN

	SET NOCOUNT ON;

	UPDATE
		dbo.Applications
	SET
		e_application_id = @EApplicationId,
		award_id = @AwardId,
		spouse_id = @SpouseId,
		application_source_id = @ApplicationSourceId,
		repayment_plan_status_id = @RepaymentPlanStatusId,
		disclosure_date = @DisclosureDate,
		repayment_plan_date_entered = @RepaymentPlanDateEntered,
		filing_status_id = @FilingStatusId,
		family_size = @FamilySize,
		tax_year = @TaxYear,
		loans_at_other_servicers = @LoansAtOtherServicers,
		joint_repayment_plan_request_indicator = @JointRepaymentPlan,
		adjusted_grose_income = @Agi,
		agi_reflects_current_income = @AgiReflectIncome,
		manually_submitted_income_indicator = @ManuallySubmittedIncomeIndicator,
		manually_submitted_income = @ManuallySubmittedIncome,
		supporting_documentation_required = @SupportingDocumentationRequired,
		supporting_documentation_received_date = @SupportingDocumentationRecDate,
		requested_by_borrower_indicator = @RequestedByBorrower,
		borrower_eligibility_id = @BorrowerEligibilityId,
		income_source_id = @IncomeSourceId,
		repayment_plan_reason_id = @RepaymentPlanReason,
		repayment_plan_type_requested_id = @RepaymentPlanTypeRequested,
		due_date_requested = @DueDateRequested,
		[state] = @State,
		received_date = @ReceivedDate,
		taxes_filed_flag = @TaxesFiledFlag,
		borrower_selected_lowest_plan = @BorrowerSelectedLowest,
		updated_at = GETDATE(),
		updated_by = @UpdatedBy,
		Active = @Active,
		RepaymentTypeProcessedNotSame = @RepaymentTypeProcessedNotSame,
		number_children = @NumberOfChildren,
		number_dependents = @NumberOfDependents,
		total_income = @TotalIncome,
		current_def_forb_id = @DefForbId,
		public_service_employment = @PublicServiceEmployment,
		reduced_payment_forbearance = @RPF,
		marital_status_id = @MartialStatusId,
		GradeLevel = @GradeLevel,
		IncludeSpouseInFamilySize = @IncludeSpouseInFamilySize
		
	WHERE application_id = @ApplicationId

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spUpdateApp] TO [db_executor]
    AS [dbo];

