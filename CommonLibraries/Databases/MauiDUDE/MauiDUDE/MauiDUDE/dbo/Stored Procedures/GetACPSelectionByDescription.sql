CREATE PROCEDURE [dbo].[GetACPSelectionByDescription]
	@ActivityCode CHAR(2),
	@ContactCode CHAR(2),
	@Description VARCHAR(50)
AS
	SELECT 
		TCX01 AS ACPSelection, 
		TCX04 AS DiscussionOption 
	FROM 
		ContactCode 
	WHERE 
		ActivityCode = @ActivityCode
		AND ContactCode = @ContactCode 
		AND [Description] = @Description
RETURN 0
