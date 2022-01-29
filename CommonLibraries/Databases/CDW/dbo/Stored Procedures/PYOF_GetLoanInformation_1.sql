-- =============================================
-- Author:		Bret Pehrson
-- Create date: 10/10/2014
-- Description:	Gets loan information for the Payoff Letter Fed application
-- =============================================
CREATE PROCEDURE [dbo].[PYOF_GetLoanInformation] 
	@DF_SPE_ACC_ID varchar(10)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		LN_SEQ AS LoanSequence,
		LA_CUR_PRI AS CurrentPrincipal,
		IC_LON_PGM AS LoanProgram
	FROM
		LN10_Loan
	WHERE
		DF_SPE_ACC_ID = @DF_SPE_ACC_ID
		AND LA_CUR_PRI > 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PYOF_GetLoanInformation] TO [db_executor]
    AS [dbo];

