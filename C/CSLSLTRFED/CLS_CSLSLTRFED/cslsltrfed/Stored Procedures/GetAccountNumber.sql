CREATE PROCEDURE [cslsltrfed].[GetAccountNumber]
	@AccountNumber VARCHAR(10)
AS
	SELECT
		DF_SPE_ACC_ID
	FROM
		CDW..PD10_PRS_NME
	WHERE
		@AccountNumber IN (DF_SPE_ACC_ID, DF_PRS_ID)
RETURN 0