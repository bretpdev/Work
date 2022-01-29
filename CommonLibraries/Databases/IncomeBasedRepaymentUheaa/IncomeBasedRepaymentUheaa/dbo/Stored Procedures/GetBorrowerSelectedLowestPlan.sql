CREATE PROCEDURE [dbo].[GetBorrowerSelectedLowestPlan]
	@AppId INT
AS
	
	SELECT
		borrower_selected_lowest_plan
	FROM
		[dbo].[Applications]
	WHERE
		application_id = @AppId
RETURN 0

GO


