--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0;
	DECLARE @ROWCOUNT INT = 0;
	DECLARE @ExpectedRowCount INT = 31;
	DECLARE @Disaster VARCHAR(100) = 'Ohio Severe Storms, Straight-line Winds, Tornadoes, Flooding, Landslides, And Mudslide (DR-4447)';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5), DisasterId INT)
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Mahoning
('44401'),
('44405'),
('44406'),
('44416'),
('44422'),
('44429'),
('44436'),
('44442'),
('44443'),
('44451'),
('44452'),
('44454'),
('44471'),
('44501'),
('44502'),
('44503'),
('44504'),
('44505'),
('44506'),
('44507'),
('44509'),
('44510'),
('44511'),
('44512'),
('44513'),
('44514'),
('44515'),
('44555'),
('44609'),
('44619'),
('44672')
	;
	
	UPDATE @Zips_ALL
	SET DisasterId = @DisasterID;

	--select * from @Zips_ALL;
	--31
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > 1;
	--0
	--select distinct * from @Zips_ALL;
	--31

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
	--31

	--select * from @Zips --new zips to add @ExpectedRowCount
	--31

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