﻿-- =============================================
-- Author:		JAROM RYAN
-- Create date: 05/31/2013
-- Description:	WILL GET DATA FOR AN EXISTING APPLICATION
-- =============================================
CREATE PROCEDURE [dbo].[spGetExistingApplication]

@AppId int

AS

BEGIN

	SET NOCOUNT ON;

				  
	SELECT
		application_id AS ApplicationId,
		e_application_id AS EApplicationId,
		award_id AS AwardId,
		spouse_id AS SpouseId,
		application_source_id AS ApplicationSourceId,
		repayment_plan_status_id AS RepaymentPlanStatusId,
		disclosure_date AS DisclosureDate,
		repayment_plan_date_entered AS RepaymentPlanDateEntered,
		filing_status_id AS FilingStatusId,
		family_size AS FamilySize,
		tax_year AS TaxYear,
		loans_at_other_servicers AS LoansAtOtherServicers,
		joint_repayment_plan_request_indicator AS JointRepaymentPlan,
		adjusted_grose_income AS Agi,
		agi_reflects_current_income AS AgiReflectIncome,
		manually_submitted_income_indicator AS ManuallySubmittedIncomeIndicator,
		manually_submitted_income AS ManuallySubmittedIncome,
		supporting_documentation_required AS SupportingDocumentationRequired,
		supporting_documentation_received_date AS SupportingDocumentationRecDate,
		requested_by_borrower_indicator AS RequestedByBorrower,
		borrower_eligibility_id AS BorrowerEligibilityId,
		income_source_id AS IncomeSourceId,
		repayment_plan_reason_id AS RepaymentPlanReason,
		repayment_plan_type_requested_id AS RepaymentPlanTypeRequested,
		due_date_requested AS DueDateRequested,
		[state] AS [State],
		received_date AS ReceivedDate,
		taxes_filed_flag AS TaxesFiledFlag,
		borrower_selected_lowest_plan AS BorrowerSelectedLowest,
		Active,
		RepaymentTypeProcessedNotSame,
		number_children AS NumberOfChildren,
		number_dependents AS NumberOfDependents,
		current_def_forb_id AS DefForbId,
		public_service_employment AS PublicServiceEmployment,
		reduced_payment_forbearance AS RPF,
		GradeLevel,
		IncludeSpouseInFamilySize
	FROM
		Applications
	WHERE 
		application_id = @AppId					
	
END
