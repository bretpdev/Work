--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 16

	UPDATE dasforbfed.Disasters
	SET Disaster = 'California Wildfires And High Winds (DR-4382)'
	WHERE Disaster = 'California Wildfires And High Winds'
	--1
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	DECLARE @Disaster VARCHAR(100) = 'California Wildfires And High Winds (DR-4382)'
	DECLARE @BeginDate DATE = '08/04/2018'
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate)
	select @Disaster,@DisasterID
	
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode) 
	VALUES	('95422'),
			('95423'),
			('95424'),
			('95426'),
			('95435'),
			('95443'),
			('95451'),
			('95453'),
			('95457'),
			('95458'),
			('95461'),
			('95464'),
			('95467'),
			('95485'),
			('95493')
	;
	--select * from @Zips
	--15

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

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END