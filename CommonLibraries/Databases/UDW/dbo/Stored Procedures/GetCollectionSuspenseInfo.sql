
-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/01/2013
-- Description:	This sproc will return if the borrower is currently in a collection suspense forbearance
-- =============================================
CREATE PROCEDURE [dbo].[GetCollectionSuspenseInfo] 

@AccountNumber VARCHAR(10)

AS
BEGIN

	SET NOCOUNT ON;

	SELECT DISTINCT
		LC_FOR_TYP,
		CAST(LD_FOR_BEG as datetime) AS CollectionSuspenseForbearanceBeginDate,
		cast(LD_FOR_END as datetime) AS CollectionSuspenseForbearanceEndDate
	
	FROM	
		[dbo].[FB10_Forbearance]
	WHERE 
		DF_SPE_ACC_ID = @AccountNumber
		AND [FOR_TYP] in('F-COLLECTION SUSPENSION', 'NATIONAL DISASTER')

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCollectionSuspenseInfo] TO [db_executor]
    AS [dbo];

