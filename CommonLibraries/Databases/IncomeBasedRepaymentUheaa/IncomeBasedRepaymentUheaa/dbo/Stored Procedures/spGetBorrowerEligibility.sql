-- =============================================
-- Author:		Jarom Ryan
-- Create date: 05/22/2013
-- Description:	Will get all of the data from dbo.Borrower_Eligibility
-- =============================================
CREATE PROCEDURE [dbo].[spGetBorrowerEligibility]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		eligibility_id AS EligibilityId,
		eligibility_code AS EligibilityCode,
		eligibility_description AS EligibilityDescription
	FROM 
		dbo.Borrower_Eligibility
END
