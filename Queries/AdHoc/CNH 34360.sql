USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XX
	DECLARE @DisasterID INT = X
	DECLARE @Disaster VARCHAR(XXX) = 'Alabama Storms and Tornadoes'
	DECLARE @BeginDate DATE = 'XX/XX/XXXX'
	DECLARE @Zips TABLE (ZipCode VARCHAR(X))
	INSERT INTO	@Zips (ZipCode)
	VALUES	('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX'),
			('XXXXX')

	INSERT INTO [dasforbfed].[Disasters]
		(DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active)
	VALUES(@DisasterID, @Disaster, @BeginDate, DATEADD(DAY, XX, @BeginDate), DATEADD(DAY, XXX, @BeginDate), X)
	--X

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	IF NOT EXISTS 
	(
		SELECT 
			ZipId 
		FROM 
			[dasforbfed].[Zips] ZX
			INNER JOIN @Zips ZX
				ON ZX.ZipCode = ZX.ZipCode
		WHERE
			ZX.DisasterId = @DisasterID
	)
	BEGIN
		INSERT INTO [dasforbfed].[Zips]	(ZipCode, DisasterId)
		SELECT 
			ZipCode
			,@DisasterID 
		FROM 
			@Zips
	END
	--XX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END