CREATE PROCEDURE [dbo].[GetBorrUcode]
	@AccountNumber VARCHAR(10) ,
	@LoanSeq INT 
AS
	SELECT 
		LF_LON_CUR_OWN
	FROM 
		[UDW].[dbo].[LN10_LON] LN10
		INNER JOIN [UDW].[dbo].[PD10_PRS_NME] PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
	WHERE 
		PD10.DF_SPE_ACC_ID = @AccountNumber
		AND LN10.LN_SEQ = @LoanSeq

RETURN 0
