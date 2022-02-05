CREATE PROCEDURE [dbo].[GetLoanPrograms]
AS
	SELECT
		LoanProgramId,
		LoanProgram,
		NsldsCode,
		IsDirect
	FROM
		dbo.LoanPrograms
RETURN 0
