-- =============================================
-- Author:		Jarom Ryan
-- Create date: 05/22/2013
-- Description:	Will get all of the data from dbo.Income_Source
-- =============================================
CREATE PROCEDURE [dbo].[spGetIncomeSource]
	
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		income_source_id AS SourceId,
		income_source_description AS Source
	FROM
		 dbo.Income_Source
END
