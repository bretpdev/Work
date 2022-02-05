

CREATE PROCEDURE [dbo].[GetServicerCategories]
	@AllowedUserId INT
AS
	SELECT DISTINCT
		SC.ServicerCategoryId,
		SC.ServicerCategory
	FROM
		ServicerCategory SC
		INNER JOIN
			UserMetricMapping UM
		ON
			UM.AllowedUserId = @AllowedUserId
			AND SC.ServicerCategoryId  = UM.CategoryId
		
			
	
		

		
RETURN 0