-- =============================================
-- Author:		JAROM RYAN
-- Create date: 05/29/2013
-- Description:	THIS WILL INSERT DATA INTO THE SPOUSES TABLE 
-- =============================================
CREATE PROCEDURE [dbo].[spInsertOrUpdateSpouseData]

@SpouseId INT = NULL,
@PersonRole INT = null,
@Ssn CHAR(9)= null,
@BirthDate DATE= null,
@FirstName CHAR(35)= null,
@LastName CHAR (35)= null,
@MiddleName CHAR (35)= null,
@PseudoSsn	BIT= null,
@DriversLicense CHAR(30) = null,
@StateCode CHAR(2) = null,
@SeperatedFromSpouse bit = null,
@AccessSpouseIncome bit = null,
@TaxesFiles bit = null,
@TaxYear varchar(4) = null,
@FilingStatusId int = null,
@SpouseAgi money = null,
@AgiReflectsIncome bit = null,
@SupportingDocsReq bit = null,
@SupportingDocsRecDate datetime = null,
@AltIncome money = null,
@LoansInSameRegion bit = null


AS
BEGIN

	SET NOCOUNT ON;
	
	IF(@SpouseId IS NULL)
	BEGIN
		INSERT INTO dbo.Spouses(person_role_id, SSN, birth_date, first_name, last_name, middle_name, pseudo_ssn, driver_license_or_state_id, state_code, separated_from_spouse, access_spouse_income_info, spouse_taxes_filed_flag,
		spouse_tax_year, spouse_filing_status_id, spouse_AGI, spouse_AGI_relects_current_income, spouse_support_docs_required, spouse_support_docs_recvd_date, spouse_alt_submitted_income, spouse_Loans_Same_Region)
		VALUES(@PersonRole, @Ssn, @BirthDate,@FirstName, @LastName,@MiddleName,@PseudoSsn,@DriversLicense,@StateCode, @SeperatedFromSpouse, @AccessSpouseIncome, @TaxesFiles, @TaxYear, @FilingStatusId, @SpouseAgi,
		@AgiReflectsIncome, @SupportingDocsReq, @SupportingDocsRecDate, @AltIncome, @LoansInSameRegion)
		SELECT  SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE 
			Spouses
		SET
			person_role_id = @PersonRole,
			SSN = @Ssn,
			birth_date = @BirthDate,
			first_name = @FirstName,
			middle_name = @MiddleName,
			last_name = @LastName,
			pseudo_ssn = @PseudoSsn,
			driver_license_or_state_id = @DriversLicense,
			state_code = @StateCode,
			separated_from_spouse = @SeperatedFromSpouse,
			access_spouse_income_info = @AccessSpouseIncome,
			spouse_taxes_filed_flag = @TaxesFiles,
			spouse_tax_year = @TaxYear,
			spouse_filing_status_id = @FilingStatusId,
			spouse_AGI = @SpouseAgi,
			spouse_AGI_relects_current_income = @AgiReflectsIncome,
			spouse_support_docs_required = @SupportingDocsReq,
			spouse_support_docs_recvd_date = @SupportingDocsRecDate,
			spouse_alt_submitted_income = @AltIncome,
			spouse_Loans_Same_Region = @LoansInSameRegion,
			updated_at = GETDATE()
		WHERE
			spouse_id = @SpouseId
	END	
END
