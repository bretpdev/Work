-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/17/2013
-- Description:	WILL GET THE SYSTEM CODE FOR A GIVEN ID
-- =============================================
CREATE PROCEDURE [dbo].[spGetEligibilityCode]

@Id INT

AS
BEGIN

	SELECT 
		eligibility_code
	FROM
		dbo.Borrower_Eligibility
	WHERE 
		eligibility_id = @Id
END