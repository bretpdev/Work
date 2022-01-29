CREATE PROCEDURE dbo.spSYSAPriority 
@CatOption			varchar(200),
@UrgOption			varchar(200)

AS

IF (SELECT COUNT(*) FROM GENR_REF_PriorityCatgryOps WHERE CatOption = @CatOption AND DefaultPriority IS NOT NULL) > 0
BEGIN
	SELECT DefaultPriority AS Priority FROM GENR_REF_PriorityCatgryOps WHERE CatOption = @CatOption
END
ELSE
BEGIN
	select A.Priority as Priority
	/*case
		when B.DefaultPriority is not null
		then B.DefaultPriority
		else A.Priority
	end as Priority*/
	
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
    ON OBJECT::[dbo].[spSYSAPriority] TO PUBLIC
    AS [dbo];

