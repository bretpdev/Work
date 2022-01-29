-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/11/2013
-- Description:	WILL GET AN EXISTING SPOUSE FROM THE SPOUSES TABLE FOR A GIVEN SPOUSE ID
-- =============================================
CREATE PROCEDURE [dbo].[spGetExistingSpouse]

@SpouseId INT

AS
BEGIN

	SELECT 
		spouse_id,
		person_role_id,
		SSN,
		birth_date,
		first_name,
		last_name,
		middle_name,
		pseudo_ssn,
		driver_license_or_state_id,
		state_code,
		separated_from_spouse,
		access_spouse_income_info,
		spouse_taxes_filed_flag,
		spouse_tax_year,
		spouse_filing_status_id,
		spouse_AGI,
		spouse_AGI_relects_current_income,
		spouse_support_docs_required,
		spouse_support_docs_recvd_date,
		spouse_alt_submitted_income,
		spouse_Loans_Same_Region,
		spouse_taxable_income,
		spouse_income_source_id
	FROM 
		dbo.Spouses
	WHERE 
		spouse_id = @SpouseId
END