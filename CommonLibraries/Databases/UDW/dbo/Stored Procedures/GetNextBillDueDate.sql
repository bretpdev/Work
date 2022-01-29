

-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/08/2013
-- Description:	This sproc will be used by VERFORBFED to get the next payment due date.
-- =============================================
CREATE PROCEDURE [dbo].[GetNextBillDueDate]

@AccountNumber AS VARCHAR(10)

AS
BEGIN

	SET NOCOUNT ON;

    
	SELECT 
		MAX(CAST(LD_BIL_DU_LON AS DATETIME))
	FROM
		[dbo].[BL10_Bill] bl10
	INNER JOIN [dbo].DW01_Loan dw01
		ON DW01.DF_SPE_ACC_ID = BL10.DF_SPE_ACC_ID
		AND DW01.LN_SEQ = BL10.LN_SEQ
		AND LTRIM(RTRIM(DW01.DW_LON_STA)) in ('IN REPAYMENT', 'PRE-CLAIM SUBMITTED')
	WHERE 
		LC_STA_BIL10 = 'A'
		AND bl10.DF_SPE_ACC_ID = @AccountNumber 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNextBillDueDate] TO [db_executor]
    AS [dbo];

