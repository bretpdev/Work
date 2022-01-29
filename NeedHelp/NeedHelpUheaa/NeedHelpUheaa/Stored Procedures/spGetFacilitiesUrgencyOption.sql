create PROCEDURE [dbo].[spGetFacilitiesUrgencyOption]
@UrgOption			varchar(200) = null

AS
BEGIN
	SET NOCOUNT ON;

    SELECT Urgency FROM LST_FacilitiesPriorityUrgency WHERE UrgencyOption = @UrgOption
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetFacilitiesUrgencyOption] TO [db_executor]
    AS [dbo];

