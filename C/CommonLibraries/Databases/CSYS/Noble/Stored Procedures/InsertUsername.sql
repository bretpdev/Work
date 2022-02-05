CREATE PROCEDURE [Noble].[InsertUsername]
(
	@Username VARCHAR(50),
	@TSR VARCHAR(4) = NULL,
	@LastUpdated DATETIME = NULL
)
AS
	INSERT INTO CSYS.Noble.UserList(Username,TSR,Last_Updated)
	VALUES(@Username, @TSR, @LastUpdated)
RETURN 0
