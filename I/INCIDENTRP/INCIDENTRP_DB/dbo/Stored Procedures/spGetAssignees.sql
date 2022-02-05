-- =============================================
-- Author:		Daren Beattie
-- Create date: September 9, 2011
-- Description:	Retrieves the AssignedTo attribute from every record in the DAT_Ticket table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetAssignees]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT AssignedTo
	FROM DAT_Ticket
	WHERE AssignedTo IS NOT NULL
END