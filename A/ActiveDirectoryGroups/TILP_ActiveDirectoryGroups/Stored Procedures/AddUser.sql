CREATE PROCEDURE [dbo].[AddUser]
	@UserId VARCHAR(50),
	@AuthLevel SMALLINT,
	@Valid BIT
AS
	INSERT INTO UserDat(UserID, [Password], AuthLevel, Valid)
	VALUES(@UserId, 'rIQXpOFj2fmdGw2xbTdRWg==', @AuthLevel, @Valid)
RETURN 0