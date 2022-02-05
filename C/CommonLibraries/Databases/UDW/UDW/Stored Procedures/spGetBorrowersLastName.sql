-- =============================================
-- Author:		Jarom Ryan
-- Create date: 02/19/2013
-- Description:	Will access the PD10 table and return the last name for a given borrower.
-- =============================================
create PROCEDURE [dbo].[spGetBorrowersLastName] 

@Ssn VARCHAR(9)

AS
BEGIN

	SELECT 
		DM_PRS_LST
	FROM dbo.PD10_Borrower
	WHERE BF_SSN = @Ssn
	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetBorrowersLastName] TO [db_executor]
    AS [dbo];

