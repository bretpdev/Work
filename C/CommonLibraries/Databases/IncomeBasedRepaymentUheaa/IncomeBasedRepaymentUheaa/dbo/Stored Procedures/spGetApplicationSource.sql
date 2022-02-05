-- =============================================
-- Author:		Jarom Ryan
-- Create date: 05/17/2013
-- Description:	Will get all the application source descriptions to populate
-- Application Type ComboBox in IDRUSERPRO
-- =============================================

CREATE PROCEDURE [dbo].[spGetApplicationSource]
	AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		application_source_id AS ApplicationSourceId,
		application_source AS ApplicationSourceDescription
	FROM dbo.Application_Source
	
END
