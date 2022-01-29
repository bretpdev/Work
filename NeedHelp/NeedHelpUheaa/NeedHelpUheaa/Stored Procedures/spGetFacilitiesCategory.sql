create PROCEDURE [dbo].[spGetFacilitiesCategory]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT CategoryOption as DisplayText, Category as BackgroundValue from dbo.LST_FacilitiesPriorityCatgry
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetFacilitiesCategory] TO [db_executor]
    AS [dbo];

