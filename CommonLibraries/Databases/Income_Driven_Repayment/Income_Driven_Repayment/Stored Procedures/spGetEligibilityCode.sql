-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/17/2013
-- Description:	WILL GET THE SYSTEM CODE FOR A GIVEN ID
-- =============================================
CREATE PROCEDURE [dbo].[spGetEligibilityCode]

@Id INT

AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		eligibility_code
	FROM
		dbo.Borrower_Eligibility
	WHERE 
		eligibility_id = @Id
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetEligibilityCode] TO [db_executor]
    AS [dbo];

