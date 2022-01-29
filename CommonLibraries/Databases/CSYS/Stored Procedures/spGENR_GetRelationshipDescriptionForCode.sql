-- =============================================
-- Author:		Jay Davis
-- Create date: 02/15/2013
-- Description:	Returns the relationship description for the code provided
-- =============================================
CREATE PROCEDURE spGENR_GetRelationshipDescriptionForCode
	@Code nchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  [description]
	FROM	GENR_LST_Relationships
	WHERE	code = @code
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetRelationshipDescriptionForCode] TO [db_executor]
    AS [dbo];

