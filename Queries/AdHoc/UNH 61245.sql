USE TLP
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO UserDat(UserID, [Password], AuthLevel, Valid)
	VALUES('kevans', 'rIQXpOFj2fmdGw2xbTdRWg==', 2, 1),
	('jgines', 'rIQXpOFj2fmdGw2xbTdRWg==', 2, 1),
	('cmiller', 'rIQXpOFj2fmdGw2xbTdRWg==', 2, 1),
	('dstone', 'rIQXpOFj2fmdGw2xbTdRWg==', 2, 1),
	('ksturzenegger', 'rIQXpOFj2fmdGw2xbTdRWg==', 2, 1),
	('ikozelkov', 'rIQXpOFj2fmdGw2xbTdRWg==', 2, 1),
	('kwesterman', 'rIQXpOFj2fmdGw2xbTdRWg==', 2, 1)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO UserDat(UserID, [Password], AuthLevel, Valid)
	VALUES('hhunter', 'rIQXpOFj2fmdGw2xbTdRWg==', 1, 1)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 8 AND @ERROR = 0
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