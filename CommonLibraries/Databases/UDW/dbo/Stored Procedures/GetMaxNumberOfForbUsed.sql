-- =============================================
-- Author:		JAROM RYAN	
-- Create date: 10/10/2013
-- Description:	this sproc will get the max number of forbearance that a borrower has used.
-- =============================================
CREATE PROCEDURE [dbo].[GetMaxNumberOfForbUsed] 

@AccountNumber VARCHAR(10)

AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		MAX(SUM_MONTHS.MONTHS)
	FROM 
		[dbo].[FB10_Forbearance] FORB
		INNER JOIN
		(
			SELECT
				DF_SPE_ACC_ID,
				SUM([MONTHS_USED]) AS MONTHS
			FROM
				[dbo].[FB10_Forbearance]
			WHERE 
				DF_SPE_ACC_ID = @AccountNumber
				AND LC_FOR_TYP = '05'
			GROUP BY
				DF_SPE_ACC_ID,
				LN_SEQ


		)SUM_MONTHS
		ON SUM_MONTHS.DF_SPE_ACC_ID = FORB.DF_SPE_ACC_ID
	WHERE 
		FORB.DF_SPE_ACC_ID = @AccountNumber
		AND FORB.LC_FOR_TYP = '05'

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetMaxNumberOfForbUsed] TO [db_executor]
    AS [dbo];

