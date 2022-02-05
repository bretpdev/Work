
CREATE PROCEDURE [dbo].[spSetRotationList]
	@SqlUserId		int,
	@Rotation		int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE
		LST_CourtAssignment
    SET
		Rotation = @Rotation
    WHERE
		SqlUserId = @SqlUserId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSetRotationList] TO [db_executor]
    AS [dbo];

