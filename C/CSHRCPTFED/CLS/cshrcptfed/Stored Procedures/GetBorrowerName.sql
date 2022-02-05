CREATE PROCEDURE [cshrcptfed].[GetBorrowerName]
	@AccountIdentifier	VARCHAR(10)
AS
BEGIN

	SELECT 
		LTRIM(RTRIM(DM_PRS_1)) + ' ' + LTRIM(RTRIM(DM_PRS_LST ))
	FROM
		CDW..PD10_PRS_NME 
	WHERE 
		@AccountIdentifier IN (DF_SPE_ACC_ID, DF_PRS_ID)
		
END
