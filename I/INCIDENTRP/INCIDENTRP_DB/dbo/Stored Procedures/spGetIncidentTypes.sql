-- =============================================
-- Author:		Daren Beattie
-- Create date: September 3, 2011
-- Description:	Gets the list of valid incident types.
-- =============================================
CREATE PROCEDURE [dbo].[spGetIncidentTypes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Type]
	FROM LST_IncidentType
END