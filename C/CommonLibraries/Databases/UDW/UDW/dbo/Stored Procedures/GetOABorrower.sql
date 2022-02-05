
CREATE PROCEDURE [dbo].[GetOABorrower]
	@AccountIdentifier varchar(10) 
AS
BEGIN
	SELECT
		PD10.DF_SPE_ACC_ID [AccountNumber],
		PD10.BF_SSN [Ssn],
		PD10.DM_PRS_1 [FirstName],
		PD10.DM_PRS_LST [LastName],
		PD30.DX_STR_ADR_1 [Address1],
		PD30.DX_STR_ADR_2 [Address2],
		PD30.DM_CT [City],
		CASE
			WHEN PD30.DM_FGN_CNY != '' THEN 'FC'
			ELSE PD30.DC_DOM_ST
		END [State],
		PD30.DF_ZIP_CDE [Zip],
		PD30.DM_FGN_CNY
	FROM
		PD10_Borrower PD10
		JOIN PD30_Address PD30 ON PD10.DF_SPE_ACC_ID = PD30.DF_SPE_ACC_ID
	WHERE
		(LEN(@AccountIdentifier) = 9 AND PD10.BF_SSN = @AccountIdentifier)
		OR (LEN(@AccountIdentifier) = 10 AND PD10.DF_SPE_ACC_ID = @AccountIdentifier)
END