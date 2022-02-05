CREATE PROCEDURE [dbo].[spNDHP_GetUrgencyOptions] 
AS
BEGIN

	SET NOCOUNT ON;

	SELECT UrgOption from GENR_REF_PriorityUrgencyOps

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spNDHP_GetUrgencyOptions] TO [db_executor]
    AS [dbo];

