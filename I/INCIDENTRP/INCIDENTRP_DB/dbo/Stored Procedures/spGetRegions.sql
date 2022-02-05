-- =============================================
-- Author:		Daren Beattie
-- Create date: September 2, 2011
-- Description:	Gets the list of regions from the LST_Region table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetRegions]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Region
	FROM LST_Region
END