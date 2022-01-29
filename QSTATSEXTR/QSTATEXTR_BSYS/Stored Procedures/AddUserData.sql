CREATE PROCEDURE [qstatsextr].[AddUserData]
	@RunTimeDate DATETIME,
	@Queue VARCHAR(8),
	@UserId VARCHAR(7),
	@StatusCode VARCHAR(50),
	@CountInStatus BIGINT,
	@TotalTime VARCHAR(50),
	@AvgTime VARCHAR(50)
AS
	
	INSERT INTO 
		QSTA_DAT_UserData (RunTimeDate, [Queue], UserID, StatusCode, CountInStatus, TotalTime, AvgTime)
	VALUES 
		(@RunTimeDate, @Queue, @UserID, @StatusCode, @CountInStatus, @TotalTime, @AvgTime)


RETURN 0
