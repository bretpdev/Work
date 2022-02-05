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
	@ManuallySubmittedIncome MONEY = NULL,
	@RequestedByBorrower BIT = NULL,
	@BorrowerEligibilityId INT = NULL,
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
	@TotalIncome money = null,
	@DefForbId int = null,
	@PublicServiceEmployment varchar(200) = null,
	@RPF money = null,
	@MaritalStatusId int = null,
	@GradeLevel char(1) = null,
	@IncludeSpouseInFamilySize bit = NULL,
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

	DECLARE @ERROR INT = 0

    INSERT INTO dbo.Applications (e_application_id, award_id,spouse_id, application_source_id, repayment_plan_status_id, disclosure_date, repayment_plan_date_entered,
		filing_status_id, family_size, tax_year, loans_at_other_servicers, joint_repayment_plan_request_indicator, adjusted_grose_income, manually_submitted_income, 
		requested_by_borrower_indicator, borrower_eligibility_id, repayment_plan_reason_id,repayment_plan_type_requested_id,due_date_requested,[state],
		received_date, updated_at, updated_by, created_by, taxes_filed_flag, borrower_selected_lowest_plan, Active, RepaymentTypeProcessedNotSame, number_children, 
		number_dependents, total_income, current_def_forb_id, public_service_employment, reduced_payment_forbearance, marital_status_id, GradeLevel, 
		IncludeSpouseInFamilySize, manually_submitted_income_indicator,
		[income_source_id],[agi_reflects_current_income],[taxable_income],[supporting_documentation_required], [supporting_documentation_received_date], [UserId])

	VALUES(@EApplicationId, @AwardId,@SpouseId,@ApplicationSourceId,@RepaymentPlanStatusId,@DisclosureDate,@RepaymentPlanDateEntered,
		@FilingStatusId,@FamilySize, @TaxYear,@LoansAtOtherServicers,@JointRepaymentPlan,@Agi,@ManuallySubmittedIncome,
		@RequestedByBorrower,@BorrowerEligibilityId,@RepaymentPlanReason,@RepaymentPlanTypeRequested,@DueDateRequested, @State,
		@ReceivedDate, GETDATE(), @UpdatedBy, @UpdatedBy, @TaxesFiledFlag, @BorrowerSelectedLowest, @Active, @RepaymentTypeProcessedNotSame, @NumberOfChildren, 
		@NumberOfDependents, @TotalIncome, @DefForbId, @PublicServiceEmployment, @RPF, @MaritalStatusId, @GradeLevel, 
		@IncludeSpouseInFamilySize, @manually_submitted_income_indicator,
		@income_source_id,@agi_reflects_current_income,@taxable_income,@supporting_documentation_required, @supporting_documentation_received_date, @UserId)
	
	SET @ERROR = @ERROR + @@ERROR

	DECLARE @ApplicationId INT = ISNULL(SCOPE_IDENTITY(), @@IDENTITY) --related triggers may potentially interfere with SCOPE_IDENTITY()

	SET @ERROR = @ERROR + @@ERROR

	IF @ApplicationId IS NULL OR @ERROR != 0
		ROLLBACK TRANSACTION
	ELSE
		BEGIN
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

			IF @ERROR = 0
				BEGIN
					SELECT @ApplicationId
					COMMIT TRANSACTION
				END
			ELSE
				ROLLBACK TRANSACTION
		END

END