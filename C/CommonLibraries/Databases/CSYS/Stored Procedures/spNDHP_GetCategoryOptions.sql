CREATE PROCEDURE [dbo].[spNDHP_GetCategoryOptions] 
AS
BEGIN

	SET NOCOUNT ON;

	SELECT CatOption from GENR_REF_PriorityCatgryOps

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spNDHP_GetCategoryOptions] TO [db_executor]
    AS [dbo];

