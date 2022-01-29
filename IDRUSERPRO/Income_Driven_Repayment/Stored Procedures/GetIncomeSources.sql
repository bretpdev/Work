-- =============================================
-- Author:		Jarom Ryan
-- Create date: 05/22/2013
-- Description:	Will get all of the data from dbo.Income_Source
-- =============================================
CREATE PROCEDURE [dbo].[GetIncomeSources]
	
AS
BEGIN

	SELECT 
		income_source_id AS SourceId,
		income_source_description AS SourceCode,
		income_source_friendly_description AS SourceDescription
	FROM
		 dbo.Income_Source
END