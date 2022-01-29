USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XX

INSERT INTO CLS.dasforbfed.Disasters(DisasterId, Disaster, BeginDate, EndDate, [Days], MaxEndDate, MaxDays, Active, AddedAt, AddedBy)
VALUES(XX,'Iowa Severe Storms and Flooding DR XXXX','XXXX-XX-XX','XXXX-XX-XX',XX,'XXXX-XX-XX',XX,X,GETDATE(),'CNH XXXXX')

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR --X rows

INSERT INTO CLS.dasforbfed.Zips(ZipCode, DisasterId)
VALUES('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX),('XXXXX',XX)

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR --XX rows

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END