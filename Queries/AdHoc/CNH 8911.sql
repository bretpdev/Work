USE NobleCalls
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

INSERT INTO NobleCalls.dbo.Comments(Comment)
VALUES('Purge number'),
('Unknown error')

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

INSERT INTO NobleCalls.dbo.DispositionCodeMapping(DispositionCode,ArcId,CommentId,ResponseCodeId)
VALUES('XX',X,XX,X), --Promise to pay contact
('XX',X,X,X),--Phone number disconnected
('XX',X,XX,X),--Purge number
('RS',X,XX,X),--Application Failure
('NA',X,X,X) --No Answer

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = X AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
