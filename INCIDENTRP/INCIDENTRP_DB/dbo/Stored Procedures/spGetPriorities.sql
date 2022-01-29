-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Gets the list of priorities from the LST_IncidentPriority table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetPriorities]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Priority
	FROM LST_IncidentPriority
END