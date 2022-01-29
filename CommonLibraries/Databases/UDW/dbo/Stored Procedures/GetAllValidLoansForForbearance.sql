-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/08/2013
-- Description:	This will return all loans not in invalid status for forbearance
-- =============================================
CREATE PROCEDURE [dbo].[GetAllValidLoansForForbearance] 

@AccountNumber AS VARCHAR(10)

AS
BEGIN

	SET NOCOUNT ON;

	SELECT DISTINCT
		DW1.[DW_LON_STA]
	FROM 
		[dbo].[DW01_Loan] DW1
	WHERE
	DW1.DF_SPE_ACC_ID = @AccountNumber

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllValidLoansForForbearance] TO [db_executor]
    AS [dbo];

