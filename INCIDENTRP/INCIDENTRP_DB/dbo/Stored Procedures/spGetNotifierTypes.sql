-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Returns the list of valid notifier types.
-- =============================================
CREATE PROCEDURE [dbo].[spGetNotifierTypes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Notifier
	FROM LST_NotifierType
END