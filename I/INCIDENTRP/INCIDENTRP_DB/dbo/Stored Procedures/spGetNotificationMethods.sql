-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Returns the list of valid notification methods.
-- =============================================
CREATE PROCEDURE [dbo].[spGetNotificationMethods]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Method
	FROM LST_NotificationMethod
END