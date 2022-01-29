--RUN ON UHEAASQLDB
USE NobleCalls
GO

--BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 7


INSERT INTO NobleCalls..CallCampaigns(CallCampaign, RegionId)
VALUES('CURE', 2)

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

UPDATE NobleCalls..CallCampaigns SET RegionId = 4 WHERE CallCampaign = 'OUT'

SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

UPDATE NobleCalls..CallCampaigns SET RegionId = 2 WHERE CallCampaign = 'USPN'

SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

DECLARE @CommentId INT

INSERT INTO NobleCalls..Comments(Comment)
VALUES('Third Party')

SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

SELECT @CommentId = @@IDENTITY

INSERT INTO NobleCalls..DispositionCodeMapping(DispositionCode, ArcId, CommentId, ResponseCodeId)
VALUES('TP',1, @CommentId, 3)

SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

INSERT INTO NobleCalls..Comments(Comment)
VALUES('Spanish')

SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

SELECT @CommentId = @@IDENTITY

UPDATE NobleCalls..DispositionCodeMapping SET CommentId = @CommentId WHERE DispositionCode = '9'

SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
