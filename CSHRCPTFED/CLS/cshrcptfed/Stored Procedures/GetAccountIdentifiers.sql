CREATE PROCEDURE [cshrcptfed].[GetAccountIdentifiers]
	@AccountIdentifier	VARCHAR(10)
AS
BEGIN
	SELECT 
		DF_SPE_ACC_ID [AccountNumber],
		DF_PRS_ID [Ssn]
	FROM
		CDW..PD10_PRS_NME 
	WHERE 
		@AccountIdentifier IN (DF_SPE_ACC_ID, DF_PRS_ID)
END