-- =============================================
-- Author:		JAROM RYAN
-- Create date: 05/28/2013
-- Description:	WILL INSERT DATA INTO THE APPLICATIONS TABLE
-- =============================================
CREATE PROCEDURE [dbo].[spInsertApplicationData]

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
@RepaymentPlanTypeRequested INT= NULL,
@DueDateRequested char(2)= NULL,
@TaxesFiledFlag bit = null,
@BorrowerSelectedLowest bit = null,
@State char(2)= NULL,
@ReceivedDate DATE = NULL,
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
@IncludeSpouseInFamilySize bit = null



AS
BEGIN

	SET NOCOUNT ON;

    INSERT INTO dbo.Applications (e_application_id, award_id,spouse_id, application_source_id, repayment_plan_status_id, disclosure_date, repayment_plan_date_entered,
filing_status_id, family_size, tax_year, loans_at_other_servicers, joint_repayment_plan_request_indicator, adjusted_grose_income, agi_reflects_current_income,
manually_submitted_income_indicator, manually_submitted_income, supporting_documentation_required, supporting_documentation_received_date, requested_by_borrower_indicator,
borrower_eligibility_id,income_source_id,repayment_plan_reason_id,repayment_plan_type_requested_id,due_date_requested,[state],received_date, updated_at, updated_by, created_by, taxes_filed_flag, borrower_selected_lowest_plan, Active, RepaymentTypeProcessedNotSame,
number_children, number_dependents, total_income, current_def_forb_id, public_service_employment, reduced_payment_forbearance, marital_status_id, GradeLevel, IncludeSpouseInFamilySize)

VALUES(@EApplicationId, @AwardId,@SpouseId,@ApplicationSourceId,@RepaymentPlanStatusId,@DisclosureDate,@RepaymentPlanDateEntered,@FilingStatusId,@FamilySize,
@TaxYear, @LoansAtOtherServicers,@JointRepaymentPlan,@Agi,@AgiReflectIncome,@ManuallySubmittedIncomeIndicator,@ManuallySubmittedIncome,@SupportingDocumentationRequired,
@SupportingDocumentationRecDate,@RequestedByBorrower,@BorrowerEligibilityId,@IncomeSourceId,@RepaymentPlanReason,@RepaymentPlanTypeRequested,@DueDateRequested,
@State,@ReceivedDate, GETDATE(), @UpdatedBy, @UpdatedBy, @TaxesFiledFlag, @BorrowerSelectedLowest, @Active, @RepaymentTypeProcessedNotSame, @NumberOfChildren, @NumberOfDependents, @TotalIncome, @DefForbId,
@PublicServiceEmployment, @RPF, @MartialStatusId, @GradeLevel, @IncludeSpouseInFamilySize)

SELECT  SCOPE_IDENTITY() 


END
