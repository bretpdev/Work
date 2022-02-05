create PROCEDURE [dbo].[spGetFacilitiesPriority]
@CatOption			varchar(200) = null,
@UrgOption			varchar(200) = null

AS
BEGIN
	SET NOCOUNT ON;

    SELECT Priority from dbo.LST_FacilitiesPriority where Category = @CatOption and Urgency = @UrgOption
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetFacilitiesPriority] TO [db_executor]
    AS [dbo];

