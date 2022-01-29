
CREATE PROCEDURE [dbo].[LT_TSUSBAENDSKP_Phone]
 @AccountNumber VARCHAR(10)

AS

SELECT  
		RTRIM(PD42.DN_DOM_PHN_ARA) + '-' + RTRIM(PD42.DN_DOM_PHN_XCH) + '-' + RTRIM(PD42.DN_DOM_PHN_LCL) [BorrPhone]
  FROM  
		PD10_PRS_NME PD10
	LEFT JOIN
		PD42_PRS_PHN PD42 ON PD42.DF_PRS_ID = PD10.DF_PRS_ID
 WHERE
		PD10.DF_SPE_ACC_ID = @AccountNumber

IF @@rowcount = 0
	BEGIN
		DECLARE @sprocname VARCHAR(MAX) = OBJECT_NAME(@@procid)
		RAISERROR('No data returned for Acct# %s. (%s)',16,2, @accountnumber, @sprocname)
	END