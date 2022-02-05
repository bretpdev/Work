create PROCEDURE [dbo].[spGetFacilitiesUrgencies]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT UrgencyOption as DisplayText, Urgency as BackgroundValue from dbo.LST_FacilitiesPriorityUrgency
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetFacilitiesUrgencies] TO [db_executor]
    AS [dbo];

