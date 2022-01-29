CREATE PROCEDURE [dbo].[spNDHP_FacilitiesPriority]
@CatOption			varchar(200),
@UrgOption			varchar(200)

AS
	select A.Priority as Priority	
	from dbo.NDHP_LST_FacilitiesPriorities A
	where A.Category = @CatOption
	and A.Urgency = @UrgOption