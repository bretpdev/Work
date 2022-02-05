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
	@ManuallySubmittedIncome MONEY = NULL,
	@RequestedByBorrower BIT = NULL,
	@BorrowerEligibilityId INT = NULL,
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
	@TotalIncome money = null,
	@DefForbId int = null,
	@PublicServiceEmployment varchar(200) = null,
	@RPF money = null,
	@MaritalStatusId int = null,
	@GradeLevel char(1) = null,
	@IncludeSpouseInFamilySize bit = null,
	@manually_submitted_income_indicator BIT = NULL,
	@income_source_id INT = NULL,
	@agi_reflects_current_income BIT = NULL,
	@taxable_income BIT = NULL,
	@supporting_documentation_required BIT = NULL,
	@supporting_documentation_received_date DATETIME     = NULL,
	@borrower_paystubs AdoiPaystubs READONLY,
	@spouse_paystubs AdoiPaystubs READONLY,
	@UserId VARCHAR(8)

AS
BEGIN

	BEGIN TRANSACTION
	DECLARE @Error INT = 0

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
		manually_submitted_income = @ManuallySubmittedIncome,
		requested_by_borrower_indicator = @RequestedByBorrower,
		borrower_eligibility_id = @BorrowerEligibilityId,
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
		marital_status_id = @MaritalStatusId,
		GradeLevel = @GradeLevel,
		IncludeSpouseInFamilySize = @IncludeSpouseInFamilySize,
		manually_submitted_income_indicator = @manually_submitted_income_indicator,
		income_source_id = @income_source_id,
		agi_reflects_current_income = @agi_reflects_current_income,
		taxable_income = @taxable_income,
		supporting_documentation_required = @supporting_documentation_required,
		supporting_documentation_received_date = @supporting_documentation_received_date,
		UserId = @UserId
	WHERE 
		application_id = @ApplicationId

	SET @ERROR = @ERROR + @@ERROR

	DELETE FROM
		Adoi_Paystubs
	WHERE
		application_id = @ApplicationId

	INSERT INTO
		dbo.Adoi_Paystubs (application_id, bonus, ftw, gross, overtime, pre_tax_deductions, adoi_paystub_frequency_id, employer_name, is_spouse)
	SELECT
		@ApplicationId, p.bonus, p.ftw, p.gross, p.overtime, p.pre_tax_deductions, p.adoi_paystub_frequency_id, p.employer_name, 0
	FROM
		@borrower_paystubs p
		INNER JOIN dbo.Adoi_Paystub_Frequencies freq
			ON freq.adoi_paystub_frequency_id = p.adoi_paystub_frequency_id

	SET @ERROR = @ERROR + @@ERROR

	INSERT INTO
		dbo.Adoi_Paystubs (application_id, bonus, ftw, gross, overtime, pre_tax_deductions, adoi_paystub_frequency_id, employer_name, is_spouse)
	SELECT
		@ApplicationId, p.bonus, p.ftw, p.gross, p.overtime, p.pre_tax_deductions, p.adoi_paystub_frequency_id, p.employer_name, 1
	FROM
		@spouse_paystubs p
		INNER JOIN dbo.Adoi_Paystub_Frequencies freq
			ON freq.adoi_paystub_frequency_id = p.adoi_paystub_frequency_id

	SET @ERROR = @ERROR + @@ERROR

	IF (@ERROR = 0)
		BEGIN
			COMMIT TRANSACTION
			SELECT CAST(1 AS BIT) [WasSuccessful]
		END
	ELSE
		BEGIN
			ROLLBACK TRANSACTION
			SELECT CAST(0 AS BIT) [WasSuccessful]
		END

END