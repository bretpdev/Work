-- =============================================
-- Author:		Daren Beattie
-- Create date: August 8, 2011
-- Description:	Retrieves the full list of business functions from GENR_LST_BusinessFunctions.
-- =============================================
CREATE PROCEDURE spGENR_GetBusinessFunctions
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT BusinessFunction
	FROM GENR_LST_BusinessFunctions
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetBusinessFunctions] TO [db_executor]
    AS [dbo];

