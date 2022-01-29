-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.testTableSproc 
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @table1 table (test1 int) 
    -- Insert statements for procedure here
	SELECT * from @table1
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[testTableSproc] TO [db_executor]
    AS [dbo];

