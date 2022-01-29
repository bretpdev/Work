-- =============================================
-- Author:		Daren Beattie
-- Create date: August 5, 2011
-- Description:	Retrieves business unit names
-- =============================================
CREATE PROCEDURE [dbo].[spGENR_GetBusinessUnits]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ID, [Name]
	FROM GENR_LST_BusinessUnits
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetBusinessUnits] TO [db_executor]
    AS [dbo];

