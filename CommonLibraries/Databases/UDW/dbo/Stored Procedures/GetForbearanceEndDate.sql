
-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/09/2013
-- Description:	This sproc will be used to get the a borrowers forbearance end date for the VERFORBFED script
-- =============================================
CREATE PROCEDURE [dbo].[GetForbearanceEndDate] 

@AccountNumber VARCHAR(10)

AS
BEGIN

	SET NOCOUNT ON;

    
	SELECT
		MAX(CAST(LD_FOR_END AS DATETIME))
	FROM
		[dbo].[FB10_Forbearance]
	WHERE 
		DF_SPE_ACC_ID = @AccountNumber
		and LC_FOR_TYP not in ('25','28')

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetForbearanceEndDate] TO [db_executor]
    AS [dbo];

