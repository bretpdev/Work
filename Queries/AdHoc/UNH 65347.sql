--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0,
			@ROWCOUNT INT = 0,
			@ExpectedRowCount INT = 13,
			@Disaster VARCHAR(100) = 'Puerto Rico Earthquakes (DR-4473)';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5), DisasterId INT)
	INSERT INTO @Zips_ALL (ZipCode) VALUES
	--Adjuntas	Cabo Rojo	Corozal		Jayuya		Lajas		Lares		Maricao		San Sebastian	Utuado		Villalba
	('00601'),	('00622'),	('00783'),	('00664'),	('00667'),	('00631'),	('00606'),	('00685'),		('00611'),	('00766'),
				('00623'),										('00669'),								('00641')
	;
	
	UPDATE @Zips_ALL
	SET DisasterId = @DisasterID;

	--select * from @Zips_ALL;
	----13
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > 1;
	----0
	--select distinct * from @Zips_ALL;
	----13

	--remove zips that already exist in database
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode)
	SELECT DISTINCT
		ZA.ZipCode
	FROM
		@Zips_ALL ZA --all zip codes from spreadsheet
		LEFT JOIN [dasforbfed].[Zips] DZ --zips in database
			ON ZA.ZipCode = DZ.ZipCode
			AND ZA.DisasterId = DZ.DisasterId
	WHERE
		DZ.ZipCode IS NULL --removes zips in database
	--13

	--select * from @Zips --new zips to add @ExpectedRowCount
	----13

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
		SELECT DISTINCT
			ZipCode
			,@DisasterID 
		FROM 
			@Zips
	END

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

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