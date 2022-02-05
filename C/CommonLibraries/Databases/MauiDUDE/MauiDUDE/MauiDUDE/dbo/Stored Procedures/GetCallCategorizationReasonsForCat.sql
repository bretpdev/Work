CREATE PROCEDURE [dbo].[GetCallCategorizationReasonsForCat]
	@Category varchar(200)

AS
	SELECT 
		Reason 
	FROM 
		CallCat_CatReasonREF 
	WHERE 
		Category = @Category
RETURN 0
