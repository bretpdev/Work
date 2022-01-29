CREATE PROCEDURE [dbo].[AddHistoryRecord]
	@UserName VARCHAR(128), 
	@Password VARCHAR(128), 
	@LoginTypeId INT, 
	@Notes VARCHAR(100), 
	@Requester VARCHAR(150), 
	@Action VARCHAR(50)
AS
BEGIN
	  INSERT INTO LoginHistory([UserName], [EncryptedPassword], [LoginTypeId], [Notes], [Requestor], [Action])
	  VALUES(@UserName,dbo.Encrypt(@Password), @LoginTypeId, @Notes, @Requester, @Action)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddHistoryRecord] TO [db_executor]
    AS [dbo];
