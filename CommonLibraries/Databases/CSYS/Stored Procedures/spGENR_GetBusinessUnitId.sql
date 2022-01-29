-- =============================================
-- Author:		Daren Beattie
-- Create date: August 5, 2011
-- Description:	Retrieves business unit names
-- =============================================
create PROCEDURE [dbo].[spGENR_GetBusinessUnitId]
@BU				VARCHAR(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	ID
	FROM	GENR_LST_BusinessUnits
	WHERE	Name = @BU
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetBusinessUnitId] TO [db_executor]
    AS [dbo];

