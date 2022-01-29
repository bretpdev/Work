-- =============================================
-- Author:		JAROM RYAN
-- Create date: 05/29/2013
-- Description:	THIS WILL INSERT DATA INTO THE SPOUSES TABLE 
-- =============================================
CREATE PROCEDURE [dbo].[spInsertOrUpdateSpouseData]
	@spouse_id INT = NULL,
	@person_role_id INT = null,
	@ssn CHAR(9)= null,
	@birth_date DATE= null,
	@first_name CHAR(35)= null,
	@last_name CHAR (35)= null,
	@middle_name CHAR (35)= null,
	@state_code CHAR(2) = null,
	@separated_from_spouse bit = null,
	@access_spouse_income_info bit = null,
	@spouse_taxes_filed_flag bit = null,
	@spouse_tax_year varchar(4) = null,
	@spouse_filing_status_id int = null,
	@spouse_AGI money = null,
	@spouse_AGI_relects_current_income bit = null,
	@spouse_support_docs_required bit = null,
	@spouse_support_docs_recvd_date datetime = null,
	@spouse_alt_submitted_income money = null,
	@spouse_Loans_Same_Region bit = null,
	@spouse_taxable_income BIT = null,
	@spouse_income_source_id INT = null
AS
BEGIN

	IF(@spouse_id IS NULL)
	BEGIN
		INSERT INTO dbo.Spouses(person_role_id, SSN, birth_date, first_name, last_name, middle_name, state_code, separated_from_spouse, access_spouse_income_info, spouse_taxes_filed_flag,
		spouse_tax_year, spouse_filing_status_id, spouse_AGI, spouse_AGI_relects_current_income, spouse_support_docs_required, spouse_support_docs_recvd_date, spouse_alt_submitted_income, spouse_Loans_Same_Region,
		spouse_taxable_income, spouse_income_source_id)
		VALUES(@person_role_id, @ssn, @birth_date,@first_name, @last_name,@middle_name,@state_code, @separated_from_spouse, @access_spouse_income_info, @spouse_taxes_filed_flag, @spouse_tax_year, @spouse_filing_status_id, @spouse_AGI,
		@spouse_AGI_relects_current_income, @spouse_support_docs_required, @spouse_support_docs_recvd_date, @spouse_alt_submitted_income, @spouse_Loans_Same_Region,
		@spouse_taxable_income, @spouse_income_source_id)
		SELECT  CAST(SCOPE_IDENTITY() AS INT)
	END
	ELSE
	BEGIN
		UPDATE 
			Spouses
		SET
			person_role_id = @person_role_id,
			SSN = @ssn,
			birth_date = @birth_date,
			first_name = @first_name,
			middle_name = @middle_name,
			last_name = @last_name,
			state_code = @state_code,
			separated_from_spouse = @separated_from_spouse,
			access_spouse_income_info = @access_spouse_income_info,
			spouse_taxes_filed_flag = @spouse_taxes_filed_flag,
			spouse_tax_year = @spouse_tax_year,
			spouse_filing_status_id = @spouse_filing_status_id,
			spouse_AGI = @spouse_AGI,
			spouse_AGI_relects_current_income = @spouse_AGI_relects_current_income,
			spouse_support_docs_required = @spouse_support_docs_required,
			spouse_support_docs_recvd_date = @spouse_support_docs_recvd_date,
			spouse_alt_submitted_income = @spouse_alt_submitted_income,
			spouse_Loans_Same_Region = @spouse_Loans_Same_Region,
			spouse_taxable_income = @spouse_taxable_income,
			spouse_income_source_id = @spouse_income_source_id,
			updated_at = GETDATE()
		WHERE
			spouse_id = @spouse_id

		SELECT @spouse_id
	END	
END