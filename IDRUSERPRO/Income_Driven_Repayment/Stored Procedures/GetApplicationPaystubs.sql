CREATE PROCEDURE [dbo].[GetApplicationPaystubs]
	@application_id INT,
	@is_spouse BIT
AS
	
	SELECT
		adoi_paystub_id,
		ftw,
		gross,
		pre_tax_deductions [total_pre_tax],
		bonus,
		overtime,
		adoi_paystub_frequency_id,
		employer_name
	FROM
		Adoi_Paystubs
	WHERE
		application_id = @application_id
		AND is_spouse = @is_spouse

RETURN 0
