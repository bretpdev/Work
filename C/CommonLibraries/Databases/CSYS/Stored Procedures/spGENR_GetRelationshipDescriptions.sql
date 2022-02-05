-- =============================================
-- Author:		Jay Davis
-- Create date: February 25, 2013
-- Description:	Returns a list of relationship descriptions
-- =============================================
create PROCEDURE [dbo].[spGENR_GetRelationshipDescriptions]
	-- Add the parameters for the stored procedure here
	@IncludeTerritories BIT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	[description]
	FROM	GENR_LST_Relationships
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetRelationshipDescriptions] TO [db_executor]
    AS [dbo];

