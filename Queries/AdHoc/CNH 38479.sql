--run on UHEAASQLDB
USE CLS;
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X;
	DECLARE @ROWCOUNT INT = X;
	DECLARE @ExpectedRowCount INT = X;
	DECLARE @Disaster VARCHAR(XXX) = 'South Dakota Severe Winter Storm, Snowstorm, And Flooding (DR-XXXX)';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(X),DisasterID INT);
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Turner
('XXXXX'),
('XXXXX'),
('XXXXX'),
('XXXXX'),
('XXXXX'),
('XXXXX'),
('XXXXX'),
('XXXXX')
	;

	--adds disaster id to zip code list
	UPDATE @ZIPS_ALL
	SET DISASTERID = @DISASTERID;

	--select * from @Zips_ALL;
	--X

	--check for dupes:
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > X
	--X hits
	--select * from @Zips_ALL
	--X
	--select distinct * from @Zips_ALL
	--X


	--remove zips that already exist in database
	DECLARE @Zips TABLE (ZipCode VARCHAR(X))
	INSERT INTO @Zips (ZipCode)
	SELECT DISTINCT
		ZA.ZipCode
	FROM
		@Zips_ALL ZA --all zip codes from spreadsheet
		LEFT JOIN [dasforbfed].[Zips] DZ --zips in database
			ON ZA.ZipCode = DZ.ZipCode
			AND DZ.DisasterID = @DisasterID
	WHERE
		DZ.ZipCode IS NULL --removes zips in database

	--select * from @Zips
	--X

	----already exist in database
	--SELECT ZipCode,DisasterID FROM @Zips_ALL
	--INTERSECT
	--SELECT ZipCode,DisasterId FROM dasforbfed.Zips WHERE DisasterId = @DisasterID
	----X

	--don't exist in database that we want to add
	--DECLARE @Zips TABLE (ZipCode VARCHAR(X))
	--INSERT INTO @Zips (ZipCode)
	--SELECT DISTINCT ZipCode
	--FROM
	--(
	--	SELECT ZipCode,DisasterID FROM @Zips_ALL
	--	EXCEPT
	--	SELECT ZipCode,DisasterId FROM dasforbfed.Zips WHERE DisasterId = @DisasterID
	--)EX
	--XX

	--select * from @Zips
	----XX

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
		SELECT DISTINCT
			ZipCode
			,@DisasterID 
		FROM 
			@Zips
	END

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END