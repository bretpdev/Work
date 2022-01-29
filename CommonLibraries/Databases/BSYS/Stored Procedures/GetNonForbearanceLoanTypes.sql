CREATE PROCEDURE [dbo].[GetNonForbearanceLoanTypes]
	
AS
	SELECT 
		LoanType 
	FROM 
		GENR_REF_LoanTypes 
	WHERE 
		TypeKey = 'NonForb'
RETURN 0
