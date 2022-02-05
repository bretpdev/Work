create PROCEDURE [dbo].[spGetFacilitiesCategoryOption]
@CatOption			varchar(200) = null

AS
BEGIN
	SET NOCOUNT ON;

    SELECT Category FROM LST_FacilitiesPriorityCatgry WHERE CategoryOption = @CatOption
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetFacilitiesCategoryOption] TO [db_executor]
    AS [dbo];

