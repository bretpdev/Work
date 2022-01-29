CREATE PROCEDURE [dbo].[UpdateUser]
	@UserId VARCHAR(50),
	@AuthLevel SMALLINT,
	@Valid BIT
AS
	UPDATE
		UserDat
	SET
		AuthLevel = @AuthLevel,
		Valid = @Valid
	WHERE
		UserID = @UserId
RETURN 0