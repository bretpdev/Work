CREATE PROCEDURE [dbo].[GetCallCategorizationCategories]

AS
	SELECT 
		Category 
	FROM 
		CallCat_Categories 
	WHERE 
		BusinessUnit IN ('B', 'C')
RETURN 0
