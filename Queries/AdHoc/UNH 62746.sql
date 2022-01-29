--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0;
	DECLARE @ROWCOUNT INT = 0;
	DECLARE @ExpectedRowCount INT = 57;
	DECLARE @Disaster VARCHAR(100) = 'Missouri Severe Storms, Tornadoes, And Flooding (DR-4451)';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5), DisasterId INT)
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Callaway	Jefferson Lewis		Newton	Saline
('63388'),('63010'),('63435'),('64840'),('65320'),
('65043'),('63012'),('63438'),('64842'),('65321'),
('65059'),('63016'),('63440'),('64844'),('65330'),
('65063'),('63019'),('63447'),('64850'),('65339'),
('65067'),('63020'),('63448'),('64853'),('65340'),
('65077'),('63023'),('63452'),('64858'),('65344'),
('65080'),('63028'),('63457'),('64864'),('65347'),
('65231'),('63030'),('63473'),('64865'),('65349'),
('65251'),('63047'),		  ('64866'),('65351'),
('65262'),('63048'),		  ('64867'),
		  ('63049'),			
		  ('63050'),			
		  ('63051'),			
		  ('63052'),			
		  ('63053'),			
		  ('63057'),			
		  ('63065'),			
		  ('63066'),			
		  ('63070')
	;
	
	UPDATE @Zips_ALL
	SET DisasterId = @DisasterID;

	--select * from @Zips_ALL;
	--56
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > 1;
	--0
	--select distinct * from @Zips_ALL;
	--56

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
	--56

	--select * from @Zips --new zips to add @ExpectedRowCount
	--56

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


	--activate disaster since disaster is currently not active
	UPDATE 
		dasforbfed.Disasters
	SET 
		Active = 1
	WHERE
		DisasterId = @DisasterID
	--1

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