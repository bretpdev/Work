USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @Expected INT = XX
	
	select * from cls.dasforbfed.Disasters
	INSERT INTO CLS.dasforbfed.Disasters(DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, MaxDays, Active, AddedAt, AddedBy)
	VALUES(X, 'North Carolina Tornado And Severe Storms', 'XXXX-XX-XX', DATEADD(DAY,XX,'XXXX-XX-XX'), DATEADD(DAY,XXX,'XXXX-XX-XX'), X, GETDATE(), SUSER_SNAME())

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	DECLARE @DisasterId INT = (SELECT DisasterId from CLS.dasforbfed.Disasters where Disaster = 'North Carolina Tornado And Severe Storms')
	INSERT INTO CLS.dasforbfed.Zips(ZipCode, DisasterId)
	VALUES
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId),
	('XXXXX',@DisasterId)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @Expected AND @ERROR = X
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