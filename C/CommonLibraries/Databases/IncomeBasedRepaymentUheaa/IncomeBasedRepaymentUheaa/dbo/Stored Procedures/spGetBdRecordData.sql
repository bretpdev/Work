
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetBdRecordData]

@StartDate DateTime = NULL,
@EndDate DateTime = NULL

AS
BEGIN

	SET NOCOUNT ON;

	SELECT DISTINCT
		BDHIS.application_id AS ApplicationId,
		BDHIS.e_application_id AS EApplicationId,
		BDHIS.award_id AS AwardId,
		application_received_date AS BDApplicationReceivedDate,
		RPR.repayment_plan_reason_code AS BDRepaymentPlanRequestCode,
		BDHIS.loans_at_other_servicers AS BDLoansAtOtherServicer,
		BDHIS.spouse_id AS BDSpouseId,
		joint_repayment_plan_request AS BDJointRepaymentPlan,
		BDHIS.tax_year AS BDTaxYear,
		BDHIS.filing_status_id AS BDFilingStatusCode,
		adjusted_gross_income AS BDAgi,
		BDHIS.agi_reflects_current_income AS BDAgiReflectsCurrentIncome,
		BDHIS.manually_submitted_income_indicator AS BDManuallySubmittedIncomeIndicator,
		BDHIS.manually_submitted_income AS BDTaxableIncome,
		BDHIS.supporting_documentation_required AS BDAppSupportingDocRequired,
		BDHIS.supporting_documentation_received_date AS BDAppSupportingDocRecDate,
		BDHIS.borrower_selected_lowest_plan AS BorrowerSelectedLowestPlan,
		BDHIS.taxes_filed_flag AS TaxesFiledFlag,
		ISNULL(DEF.file_value, ' ') as CurrentDefForbOpt,
		BDHIS.public_service_employment AS PublicServiceIndicator,
		BDHIS.reduced_payment_forbearance AS ReducedPaymentForb,
		BDHIS.number_children AS NumberOfChildren,
		BDHIS.number_dependents AS NumberOfDependents,
		BDHIS.marital_status_id AS MaritalStatus,
		S.separated_from_spouse AS SeperatedFromSpouse,
		S.access_spouse_income_info AS AccessToSpouseIncome,
		S.spouse_taxes_filed_flag AS SpousesTaxesFiled,
		S.spouse_tax_year AS SpouseTaxYear,
		S.spouse_filing_status_id AS SpouseFilingStatusId,
		S.spouse_AGI AS SpouseAgi,
		S.spouse_AGI_relects_current_income AS SpouseAgiReflectsIncome,
		S.spouse_support_docs_required AS SpouseSupDocsReq,
		S.spouse_support_docs_recvd_date as SpouseSupDocRecDate,
		S.spouse_alt_submitted_income AS SpouseIncome,
		ISNULL(LNS.loan_seq, '    ')  AS LoanSeq
	
	
	FROM 
		dbo.BD_Data_History BDHIS
	LEFT JOIN dbo.Repayment_Plan_Reason RPR
		ON RPR.repayment_plan_reason_id = BDHIS.repayment_plan_reason_id 
	LEFT JOIN dbo.Loans LNS
		ON LNS.application_id = BDHIS.application_id
		AND lns.award_id = BDHIS.award_id
	INNER JOIN [dbo].[Applications] APP
		ON APP.application_id = BDHIS.application_id
		AND APP.Active = 1
	LEFT JOIN Spouses S
		ON BDHIS.spouse_id = S.spouse_id
	LEFT JOIN Current_Def_Forb_Options DEF
		ON DEF.current_def_forb_option_Id = BDHIS.current_def_forb_id
	WHERE
		BDHIS.created_at BETWEEN @StartDate AND @EndDate
	
END


GRANT EXECUTE ON [dbo].[spGetBdRecordData] TO db_executor
