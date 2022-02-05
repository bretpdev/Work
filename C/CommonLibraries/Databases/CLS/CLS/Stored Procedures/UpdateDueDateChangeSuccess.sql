CREATE PROCEDURE [dbo].[UpdateDueDateChangeSuccess]
	@DueDateChangeId int

AS
	update
		DueDateChange
	set
		Successful = 1
	where
		DueDateChangeId = @DueDateChangeId
RETURN 0
