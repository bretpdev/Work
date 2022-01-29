﻿-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/10/2013
-- Description:	This sproc will return the account number if the given accountnumber has a future dated forbearance
-- =============================================
CREATE PROCEDURE [dbo].[CheckForFutureDateDeferment]

@AccountNumber VARCHAR(10)

AS
BEGIN

	SET NOCOUNT ON;

	SELECT DISTINCT
		DF_SPE_ACC_ID
	FROM
		[dbo].[DF10_Deferment]
	WHERE 
		DF_SPE_ACC_ID = @AccountNumber
		AND LD_DFR_BEG > GETDATE()


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForFutureDateDeferment] TO [db_executor]
    AS [dbo];

