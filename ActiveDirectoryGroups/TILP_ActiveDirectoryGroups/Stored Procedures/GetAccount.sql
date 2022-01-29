CREATE PROCEDURE [dbo].[GetAccount]
	@UserId VARCHAR(50)
AS
	SELECT
		U.UserID,
		A.LevelDesc,
		U.Valid
	FROM
		UserDat U
		LEFT JOIN AuthList A
			ON U.AuthLevel = A.AuthLevel
	WHERE
		U.UserID = @UserId
RETURN 0