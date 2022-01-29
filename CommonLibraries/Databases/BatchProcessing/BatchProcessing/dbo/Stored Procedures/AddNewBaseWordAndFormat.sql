CREATE PROCEDURE [dbo].[AddNewBaseWordAndFormat]
	@BaseWord char(4),
	@Format char(16)
AS
	UPDATE
		BatchPasswordBase
	SET
		InactivatedAt = GETDATE()
	WHERE
		InactivatedAt IS NULL

	INSERT INTO BatchPasswordBase(BaseWord, [Format])
	VALUES(dbo.Encrypt(@BaseWord), dbo.Encrypt(@Format))
RETURN 0
