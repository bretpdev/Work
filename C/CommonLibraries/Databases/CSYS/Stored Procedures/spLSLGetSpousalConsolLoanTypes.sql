-- =============================================
-- Author:		Jarom Ryan
-- Create date: 01/08/2013
-- Description:	Will get the loan types from dbo.LSL_LST_SpousalConsolLoans
-- =============================================
CREATE PROCEDURE [dbo].[spLSLGetSpousalConsolLoanTypes]


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Select LoanType
	From dbo.LSL_LST_SpousalConsolLoans
	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLSLGetSpousalConsolLoanTypes] TO [db_executor]
    AS [dbo];

