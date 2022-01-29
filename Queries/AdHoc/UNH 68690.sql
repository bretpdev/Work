--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0,
			@ROWCOUNT INT = 0,
			@ExpectedRowCount INT = 117,
			@Disaster VARCHAR(100) = 'Louisiana Hurricane Laura (DR-4559-LA)';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbuh.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbuh.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5), DisasterId INT)
	INSERT INTO @Zips_ALL (ZipCode) VALUES
--Affected Zip Codes
('70510'),
('70511'),
('70516'),
('70525'),
('70526'),
('70527'),
('70528'),
('70531'),
('70533'),
('70534'),
('70537'),
('70542'),
('70543'),
('70548'),
('70555'),
('70556'),
('70559'),
('70575'),
('70578'),
('71002'),
('71031'),
('71065'),
('71066'),
('71201'),
('71202'),
('71203'),
('71207'),
('71209'),
('71210'),
('71211'),
('71212'),
('71213'),
('71217'),
('71225'),
('71226'),
('71227'),
('71235'),
('71238'),
('71240'),
('71245'),
('71247'),
('71251'),
('71268'),
('71270'),
('71272'),
('71273'),
('71275'),
('71280'),
('71281'),
('71291'),
('71292'),
('71294'),
('71301'),
('71302'),
('71303'),
('71306'),
('71307'),
('71309'),
('71315'),
('71325'),
('71328'),
('71330'),
('71346'),
('71348'),
('71359'),
('71360'),
('71361'),
('71365'),
('71404'),
('71405'),
('71406'),
('71407'),
('71409'),
('71410'),
('71411'),
('71414'),
('71416'),
('71417'),
('71419'),
('71422'),
('71423'),
('71424'),
('71426'),
('71427'),
('71428'),
('71429'),
('71430'),
('71431'),
('71432'),
('71433'),
('71434'),
('71438'),
('71440'),
('71447'),
('71448'),
('71449'),
('71450'),
('71452'),
('71454'),
('71455'),
('71456'),
('71457'),
('71458'),
('71460'),
('71462'),
('71466'),
('71467'),
('71468'),
('71469'),
('71471'),
('71472'),
('71473'),
('71477'),
('71483'),
('71485'),
('71486'),
('71497')

	;
	
	UPDATE @Zips_ALL
	SET DisasterId = @DisasterID;

	--select * from @Zips_ALL;
	----117
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > 1;
	----0
	--select distinct * from @Zips_ALL;
	----117

	--remove zips that already exist in database
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode)
	SELECT DISTINCT
		ZA.ZipCode
	FROM
		@Zips_ALL ZA --all zip codes from spreadsheet
		LEFT JOIN [dasforbuh].[Zips] DZ --zips in database
			ON ZA.ZipCode = DZ.ZipCode
			AND ZA.DisasterId = DZ.DisasterId
	WHERE
		DZ.ZipCode IS NULL --removes zips in database
	--117

	--select * from @Zips --new zips to add @ExpectedRowCount
	--117

	IF NOT EXISTS 
	(
		SELECT 
			ZipId 
		FROM 
			[dasforbuh].[Zips] Z1
			INNER JOIN @Zips Z2
				ON Z1.ZipCode = Z2.ZipCode
		WHERE
			Z1.DisasterId = @DisasterID
	)
	BEGIN
		INSERT INTO [dasforbuh].[Zips]	(ZipCode, DisasterId)
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