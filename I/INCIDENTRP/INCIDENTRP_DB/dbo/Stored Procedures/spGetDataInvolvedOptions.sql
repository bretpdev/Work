-- =============================================
-- Author:		Daren Beattie
-- Create date: September 1, 2011
-- Description:	Returns a list of types of data that can be involved in an incident.
-- =============================================
CREATE PROCEDURE [dbo].[spGetDataInvolvedOptions]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Type]
	FROM LST_DataInvolved
END