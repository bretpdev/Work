--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 32
	DECLARE @Disaster VARCHAR(100) = 'Georgia Hurricane Michael (DR-4400)'
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate)
	--select @Disaster,@DisasterID,@BeginDate

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Crisp		Grady		Lee			Terrel		Thomas		Worth
('31015'),	('39897'),	('31763'),	('39842'),	('37138'),	('31772'),
('31010'),	('39828'),	('31787'),	('39826'),	('31757'),	('31781'),
('31712'),	('39827'),				('39885'),	('31626'),	('31789'),
			('39829'),				('39877'),	('31799'),	('31796'),
												('31758'),	('31792'),
												('31792'),	
												('31778'),	
												('31765'),	
												('31773'),
--extras
('31730'),
('31716'),
('31784'),
('31779'),
('31739')
	;
	--select * from @Zips_ALL
	--32

	--remove zips that already exist in database
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode)
	SELECT 
		ZA.ZipCode
	FROM
		@Zips_ALL ZA --all zip codes from spreadsheet
		LEFT JOIN [dasforbfed].[Zips] DZ --zips in database
			ON ZA.ZipCode = DZ.ZipCode
	WHERE
		DZ.ZipCode IS NULL --removes zips in database
	--32

	--select * from @Zips
	--32

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