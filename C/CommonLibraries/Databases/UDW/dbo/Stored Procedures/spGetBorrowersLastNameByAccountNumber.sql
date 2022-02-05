-- =============================================
-- Author:		Jarom Ryan
-- Create date: 02/19/2013
-- Description:	Will access the PD10 table and return the last name for a given borrower.
-- =============================================
CREATE PROCEDURE [dbo].[spGetBorrowersLastNameByAccountNumber] 

@AccountNumber VARCHAR(10)

AS
BEGIN

	SELECT 
		DM_PRS_LST
	FROM 
		dbo.PD10_Borrower
	WHERE
		DF_SPE_ACC_ID = @AccountNumber
	
END