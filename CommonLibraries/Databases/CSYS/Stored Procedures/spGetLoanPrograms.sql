-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/17/2013
-- Description:	WILL GET LOAN PROGRAMS FROM GENR_LST_Loan_Program
-- =============================================
CREATE PROCEDURE [dbo].[spGetLoanPrograms] 

AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		loan_program AS LoanProgram
	FROM
		dbo.GENR_LST_Loan_Program
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetLoanPrograms] TO [db_executor]
    AS [dbo];

