CREATE PROCEDURE [dbo].[spNDHP_GetPriority] 
@CatOption			varchar(200) = null,
@UrgOption			varchar(200) = null

AS

IF (SELECT COUNT(*) FROM GENR_REF_PriorityCatgryOps WHERE CatOption = @CatOption AND DefaultPriority IS NOT NULL) > 0
BEGIN
	SELECT cast(DefaultPriority as smallint) AS Priority FROM GENR_REF_PriorityCatgryOps WHERE CatOption = @CatOption
END
ELSE
BEGIN
	select A.Priority as Priority
	
	from GENR_LST_Priorities A
	inner join GENR_REF_PriorityCatgryOps B
		on A.Category = B.Category
	inner join GENR_REF_PriorityUrgencyOps C
		on A.Urgency = C.Urgency
	where B.CatOption = @CatOption
	and C.UrgOption = @UrgOption
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spNDHP_GetPriority] TO [db_executor]
    AS [dbo];

