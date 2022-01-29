-- =============================================
-- Author:		Daren Beattie
-- Create date: September 9, 2011
-- Description:	Retrieves the Status attribute from all records in the DAT_Ticket table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetStatuses]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT [Status]
	FROM DAT_Ticket
END