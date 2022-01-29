--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0;
	DECLARE @ROWCOUNT INT = 0;
	DECLARE @ExpectedRowCount INT = 47;
	DECLARE @Disaster VARCHAR(100) = 'Oklahoma Severe Storms, Straight-line Winds, Tornadoes, And Flooding (DR-4438)';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5), DisasterId INT)
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Alfalfa	Craig	Garfield	Kingfisher	Pawnee	Woods
('73716'),('74301'),	('73701'),('73016'),('74020'),('73717'),
('73719'),('74332'),	('73702'),('73734'),('74034'),('73731'),
('73722'),('74369'),	('73703'),('73742'),('74038'),('73746'),
('73726'),				('73705'),('73750'),('74045'),('73842'),
('73728'),				('73706'),('73756'),('74058'),('73860'),
('73739'),				('73720'),('73762'),('74081'),	
('73741'),				('73727'),('73764'),('74650'),	
('73749'),				('73730'),			
						('73733'),			
						('73735'),			
						('73736'),			
						('73738'),			
						('73743'),			
						('73753'),			
						('73754'),			
						('73773'),			
						('74640')
	;
	
	UPDATE @Zips_ALL
	SET DisasterId = @DisasterID;

	--select * from @Zips_ALL;

	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > 1;
	--0 hit
	--select * from @Zips_ALL;
	--47
	--select distinct * from @Zips_ALL;
	--47

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
	--47

	--select * from @Zips --new zips to add @ExpectedRowCount
	--47

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