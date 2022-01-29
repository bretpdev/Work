SELECT
	*
FROM
	CLS.dasforbfed.Zips Z
	INNER JOIN CLS.dasforbfed.Disasters D
		ON Z.DisasterId = D.DisasterId
WHERE 
	Z.ZipCode IN
	(
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX',
		'XXXXX'
	)
	;











----run on UHEAASQLDB
--USE CLS
--GO

--BEGIN TRANSACTION
--	DECLARE @ERROR INT = X
--	DECLARE @ROWCOUNT INT = X
--	DECLARE @ExpectedRowCount INT = XX
--	DECLARE @Disaster VARCHAR(XXX) = 'Florida Hurricane Michael (DR-XXXX)'
--	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
--	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate)
--	--select @Disaster,@DisasterID,@BeginDate
	
--	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(X)) --all zip codes from spreadsheet
--	INSERT INTO @Zips_ALL (ZipCode) VALUES
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX'),
--('XXXXX')
--;

--	--select * from @Zips_ALL order by zipcode
--	--XX
--	--select zipcode,count(zipcode) from @Zips_ALL group by ZipCode having count(zipcode) > X
--	--select distinct * from @Zips_ALL
--	----XX

--	--remove zips that already exist in database for that disaster
--	DECLARE @Zips TABLE (ZipCode VARCHAR(X))
--	INSERT INTO @Zips (ZipCode)
--	SELECT DISTINCT
--		ZA.ZipCode
--	FROM
--		@Zips_ALL ZA --all zip codes from spreadsheet
--		LEFT JOIN [dasforbfed].[Zips] DZ --zips in database
--			ON ZA.ZipCode = DZ.ZipCode
--			AND DZ.DisasterId = @DisasterID
--	WHERE
--		DZ.ZipCode IS NULL --removes zips in database
--	--X

--	select * from @Zips
--	--X

--	--final insert into database
--	IF NOT EXISTS 
--	(
--		SELECT 
--			 ZX.ZipId
--			--,ZX.ZipCode
--		FROM 
--			[dasforbfed].[Zips] ZX
--			INNER JOIN @Zips ZX
--				ON ZX.ZipCode = ZX.ZipCode
--		WHERE
--			ZX.DisasterId = @DisasterID
--	)
--	BEGIN
--		INSERT INTO [dasforbfed].[Zips]	(ZipCode, DisasterId)
--		SELECT 
--			ZipCode
--			,@DisasterID 
--		FROM 
--			@Zips
--	END

--	-- Save/Set the row count and error number (if any) from the previously executed statement
--	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

--IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
--	BEGIN
--		PRINT 'Transaction committed'
--		COMMIT TRANSACTION
--		--ROLLBACK TRANSACTION
--	END
--ELSE
--	BEGIN
--		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
--		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
--		PRINT 'Transaction NOT committed'
--		ROLLBACK TRANSACTION
--	END