-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/11/2013
-- Description:	WILL GET AN EXISTING SPOUSE FROM THE SPOUSES TABLE FOR A GIVEN SPOUSE ID
-- =============================================
CREATE PROCEDURE [dbo].[spGetExistingSpouse]

@SpouseId INT

AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		spouse_id AS SpouseId,
		person_role_id AS PersonRole,
		SSN AS Ssn,
		birth_date AS BirthDate,
		first_name AS FirstName,
		last_name AS LastName,
		middle_name AS MiddleName,
		pseudo_ssn AS PseudoSsn,
		driver_license_or_state_id AS DriversLicense,
		state_code AS StateCode,
		separated_from_spouse as SeperatedFromSpouse,
		access_spouse_income_info as AccessSpouseIncome,
		spouse_taxes_filed_flag as TaxesFiles,
		spouse_tax_year as TaxYear,
		spouse_filing_status_id as FilingStatusId,
		spouse_AGI as SpouseAgi,
		spouse_AGI_relects_current_income as AgiReflectsIncome,
		spouse_support_docs_required as SupportingDocsReq,
		spouse_support_docs_recvd_date as SupportingDocsRecDate,
		spouse_alt_submitted_income as AltIncome,
		spouse_Loans_Same_Region as LoansInSameRegion


	FROM 
		dbo.Spouses
	WHERE 
		spouse_id = @SpouseId
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetExistingSpouse] TO [db_executor]
    AS [dbo];

