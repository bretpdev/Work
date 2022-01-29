--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 47
	DECLARE @Disaster VARCHAR(100) = 'Georgia Hurricane Michael (DR-4400)'
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate)
	--select @Disaster,@DisasterID,@BeginDate

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Baker		Calhoun		Clay		Crisp		Decatur		Dougherty	Early		Grady		Laurens		Lee			Mitchell	Miller		Randolph	Seminole	Sumter		Terrel		Thomas		Tift		Turner		Worth
('39870'),	('39813'),	('39851'),	('31015'),	('39815'),	('31708'),	('39823'),	('39897'),	('31027'),	('31763'),	('31730'),	('39837'),	('39886'),	('39845'),	('31735'),	('39842'),	('37138'),	('31727'),	('31790'),	('31772'),
			('39866'),	('39824'),	('31010'),	('39819'),	('31706'),	('39832'),	('39828'),	('31022'),	('31787'),	('31716'),				('39840'),	('39859'),	('31764'),	('39826'),	('31757'),	('31733'),	('31783'),	('31781'),
			('39862'),	('30250'),	('31712'),	('39825'),	('31707'),	('39861'),	('39827'),	('31040'),				('31784'),				('39836'),				('31743'),	('39885'),	('31626'),	('31794'),	('31714'),	('31789'),
			('39846'),	('30238'),				('39817'),	('31782'),	('39841'),	('39829'),	('31027'),				('31779'),										('31709'),	('39877'),	('31799'),	('31775'),				('31796'),
						('30274'),				('39818'),	('31705'),				('31739'),	('31021'),																('31711'),				('31758'),	('31793'),				('31792'),
						('30287'),				('39852'),	('31702'),							('31065'),																('31780'),				('31792'),	('31795'),		
						('30298'),				('39834'),	('31701'),							('30454'),																('31719'),				('31778'),			
						('30288'),							('31721'),							('31019'),																						('31765'),			
						('30296'),							('31704'),							('31009'),																						('31773'),			
						('30297'),							('31703'),							('31075'),											
						('30237'),																	
						('30236'),																	
						('30260'),																	
						('30273')
;
	--select * from @Zips_ALL
	--104
	--select zipcode,count(zipcode) from @Zips_ALL group by ZipCode having count(zipcode) > 1
	--select distinct * from @Zips_ALL
	--102

	--remove zips that already exist in database
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode)
	SELECT 
		ZA.ZipCode
	FROM
		@Zips_ALL ZA --all zip codes from spreadsheet
		LEFT JOIN [dasforbfed].[Zips] DZ --zips in database
			ON ZA.ZipCode = DZ.ZipCode
			AND DZ.DisasterId = @DisasterID
	WHERE
		DZ.ZipCode IS NULL --removes zips in database
	--47

	--select * from @Zips
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