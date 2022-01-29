USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 44
	DECLARE @DisasterID INT = 5
	DECLARE @Disaster VARCHAR(100) = 'Alabama Storms and Tornadoes'
	DECLARE @BeginDate DATE = '04/26/2018'
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO	@Zips (ZipCode)
	VALUES	('36201'),
			('36202'),
			('36203'),
			('36204'),
			('36205'),
			('36206'),
			('36207'),
			('36210'),
			('36250'),
			('36253'),
			('36254'),
			('36257'),
			('36260'),
			('36265'),
			('36271'),
			('36272'),
			('36277'),
			('36279'),
			('35019'),
			('35033'),
			('35053'),
			('35055'),
			('35056'),
			('35057'),
			('35058'),
			('35070'),
			('35077'),
			('35083'),
			('35087'),
			('35098'),
			('35179'),
			('35901'),
			('35902'),
			('35903'),
			('35904'),
			('35905'),
			('35906'),
			('35907'),
			('35952'),
			('35954'),
			('35956'),
			('35972'),
			('35990')

	INSERT INTO [dasforbfed].[Disasters]
		(DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active)
	VALUES(@DisasterID, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 179, @BeginDate), 1)
	--1

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	IF NOT EXISTS 
	(
		SELECT 
			ZipId 
		FROM 
			[dasforbfed].[Zips] Z1
			INNER JOIN @Zips Z2
				ON Z1.ZipCode = Z2.ZipCode
		WHERE
			Z1.DisasterId = @DisasterID
	)
	BEGIN
		INSERT INTO [dasforbfed].[Zips]	(ZipCode, DisasterId)
		SELECT 
			ZipCode
			,@DisasterID 
		FROM 
			@Zips
	END
	--43

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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