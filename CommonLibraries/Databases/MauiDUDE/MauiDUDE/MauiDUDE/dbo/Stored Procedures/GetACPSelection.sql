CREATE PROCEDURE [dbo].[GetACPSelection]
	@ActivityCode CHAR(2),
	@ContactCode CHAR(2)
AS
	SELECT 
		TCX01 AS ACPSelection, 
		TCX04 AS DiscussionOption 
	FROM 
		ContactCode 
	WHERE 
		ActivityCode = @ActivityCode
		AND ContactCode = @ContactCode 
RETURN 0
