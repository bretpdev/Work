CREATE PROCEDURE [dbo].[GetAdoiIncome]
	@adoi_income_id INT
AS

	SELECT
		income_source_id,
		income_changed,
		income_taxable,
		supporting_docs_required,
		received_date
	FROM
		Adoi_Income
	WHERE
		adoi_income_id = @adoi_income_id

RETURN 0
