-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/09/2013
-- Description:	This sproc will be used to get the a borrowers forbearance end date for the VERFORBFED script
-- =============================================
CREATE PROCEDURE [dbo].[GetDefermentEndDate] 

@AccountNumber VARCHAR(10)

AS
BEGIN

	SET NOCOUNT ON;

    
	SELECT
		MAX(CAST(LD_DFR_END AS DATETIME))
	FROM
		[dbo].[DF10_Deferment]
	WHERE 
		DF_SPE_ACC_ID = @AccountNumber

END