-- =============================================
-- Author:		Jarom Ryan	
-- Create date: 12/19/2012
-- Description:	This will pull all the Arc from the table
-- =============================================
CREATE PROCEDURE [dbo].[spGetTcpaArcs]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Arc
	FROM dbo.TCPA_LST_Arcs
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetTcpaArcs] TO [db_executor]
    AS [dbo];

