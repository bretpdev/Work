-- =============================================
-- Author:		Jay Davis
-- Create date: 02/15/2013
-- Description:	Returns the relationship description for the code provided
-- =============================================
create PROCEDURE [dbo].[spGENR_GetRelationshipCodeForDescription]
	@description varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  code
	FROM	GENR_LST_Relationships
	WHERE	[description] = @description
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetRelationshipCodeForDescription] TO [db_executor]
    AS [dbo];

