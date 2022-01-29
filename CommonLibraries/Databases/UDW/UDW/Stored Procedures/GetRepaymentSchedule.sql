-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/09/2013
-- Description:	This sproc will get the repayment schedule for a given borrower, this is being used in the VERFORBFED script
-- =============================================
CREATE PROCEDURE [dbo].[GetRepaymentSchedule] 

@AccountNumber VARCHAR(10)

AS
BEGIN

	SET NOCOUNT ON;

    SELECT
		COUNT(distinct [TYP_SCH_DIS])
	FROM
		[dbo].[LN65_RepaymentSched] LN65
	INNER JOIN IDRRepaymentSchedules IDR
		ON IDR.IDRRepaymentSchedule = LN65.TYP_SCH_DIS
	WHERE 
		DF_SPE_ACC_ID = @AccountNumber
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRepaymentSchedule] TO [db_executor]
    AS [dbo];

