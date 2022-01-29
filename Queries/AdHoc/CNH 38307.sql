--run on UHEAASQLDB
USE CLS;
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XX
	DECLARE @Disaster VARCHAR(XXX) = 'Arkansas Severe Storms And Flooding (DR-XXXX)'
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate)
	--select @Disaster,@DisasterID,@BeginDate

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(X))
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Arkansas	Desha	Logan	Pope
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),			('XXXXX'),('XXXXX'),
							  ('XXXXX')
	;
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > X
	--X hit
	--select * from @Zips_ALL
	--XX
	--select distinct * from @Zips_ALL
	--XX

	--remove zips that already exist in database
	DECLARE @Zips TABLE (ZipCode VARCHAR(X))
	INSERT INTO @Zips (ZipCode)
	SELECT DISTINCT
		ZA.ZipCode
	FROM
		@Zips_ALL ZA --all zip codes from spreadsheet
		LEFT JOIN [dasforbfed].[Zips] DZ --zips in database
			ON ZA.ZipCode = DZ.ZipCode
	WHERE
		DZ.ZipCode IS NULL --removes zips in database
	--XX

	--select * from @Zips
	--XX

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