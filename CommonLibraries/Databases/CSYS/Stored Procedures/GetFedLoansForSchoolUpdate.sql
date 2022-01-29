CREATE PROCEDURE [dbo].[GetFedLoansForSchoolUpdate]
	
AS
	SELECT 
		loan_program
	FROM
		[GENR_LST_Loan_Program]
	WHERE
		direct = 1
		and loan_program not in ('DLSPCN','DSCON','PERKNS')
RETURN 0
