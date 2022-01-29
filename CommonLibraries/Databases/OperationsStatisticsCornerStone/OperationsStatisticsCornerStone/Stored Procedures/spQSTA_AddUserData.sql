
CREATE PROCEDURE spQSTA_AddUserData
	@UserDataTable UserData ReadOnly
AS
BEGIN
SET XACT_ABORT ON
	BEGIN TRANSACTION 
	INSERT INTO QSTA_DAT_UserData (RunDateTime, [Queue], UserID, Assigned, Complete, Canceled)
		SELECT
			[RunDateTime],
			[Queue],
			[UserId],
			[Assigned],
			[Complete],
			[Canceled]
		FROM	
			@UserDataTable
	COMMIT
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spQSTA_AddUserData] TO [db_executor]
    AS [dbo];

