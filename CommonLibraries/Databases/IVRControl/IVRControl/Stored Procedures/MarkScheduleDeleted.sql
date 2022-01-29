CREATE PROCEDURE [dbo].[MarkScheduleDeleted]
	@StatusScheduleId INT,
	@DeletedBy VARCHAR(50)
AS
	UPDATE
		StatusSchedules
	SET
		DeletedBy = @DeletedBy,
		DeletedAt = GETDATE()
	WHERE 
		StatusScheduleId = @StatusScheduleId
RETURN 0
