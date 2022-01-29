USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @DisasterID INT

	INSERT INTO [CLS].[dasforbfed].[Disasters](DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active)
	VALUES(X, 'Tropical Storm Gita', 'XX/XX/XXXX', DATEADD(DAY, XX, 'XX/XX/XXXX'), DATEADD(DAY, XXX, 'XX/XX/XXXX'), X)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	IF NOT EXISTS (SELECT ZipId FROM cls.dasforbfed.Zips WHERE ZipCode = 'XXXXX' AND DisasterId = X)
	BEGIN
		INSERT INTO CLS.dasforbfed.Zips(ZipCode, DisasterId)
		VALUES('XXXXX', X)
	END

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