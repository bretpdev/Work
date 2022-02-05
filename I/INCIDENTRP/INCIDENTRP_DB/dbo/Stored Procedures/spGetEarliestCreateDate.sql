-- =============================================
-- Author:		Daren Beattie
-- Create date: September 9, 2011
-- Description:	Retrieves the earliest CreateDateTime from the DAT_Ticket table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetEarliestCreateDate]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COALESCE(MIN(CreateDateTime), GETDATE())
	FROM DAT_Ticket
END