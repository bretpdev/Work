--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 8
	DECLARE @Disaster VARCHAR(100) = 'Wisconsin Severe Storms, Tornadoes, Straight-line Winds, Flooding, And Landslides (DR-4402)'
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate)
	--select @Disaster,@DisasterID,@BeginDate

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Crawford	Dane		Juneau		La Crosse	Marquette	Monroe		Richland	Sauk 		Vernon
('54645'),	('53791'),	('54646'),	('54669'),	('53930'),	('54670'),	('53556'),	('53583'),	('54652'),
('54655'),	('53706'),	('53944'),	('54644'),	('53920'),	('54666'),	('53581'),	('53958'),	('54651'),
('54631'),	('53708'),	('53948'),	('54636'),	('54960'),	('54656'),	('54664'),	('53577'),	('54639'),
('54657'),	('53711'),	('53929'),	('54602'),	('53949'),	('54660'),	('53540'),	('53578'),	('54667'),
('53826'),	('53707'),	('54637'),	('54614'),	('53952'),	('54619'),	('53584'),	('53588'),	('54665'),
('54654'),	('53597'),	('54641'),	('54603'),	('53953'),	('54620'),	('53924'),	('53913'),	('54658'),
('53821'),	('53703'),	('54618'),	('54601'),	('53964'),	('54648'),				('53959'),	('54623'),
('54626'),	('53701'),	('53962'),	('54650'),				('54662'),				('53961'),	('54632'),
('54628'),	('53702'),	('53968'),	('54653'),				('54638'),				('53943'),	('54624'),
			('53598'),	('53950'),							('54649'),				('53942'),	('54634'),
			('53705'),																('53940'),	('54621'),
			('53529'),																('53951'),	
			('53704'),																('53561'),	
			('53725'),																('53941'),	
			('53790'),																('53937'),	
			('53719'),							
			('53718'),							
			('53523'),							
			('53744'),							
			('53508'),							
			('53515'),							
			('53517'),							
			('53726'),							
			('53528'),							
			('53715'),							
			('53714'),							
			('53794'),							
			('53713'),							
			('53783'),							
			('53717'),							
			('53527'),							
			('53716'),							
			('53774'),							
			('53596'),							
			('53562'),							
			('53784'),							
			('53792'),							
			('53558'),							
			('53777'),							
			('53786'),							
			('53571'),							
			('53572'),							
			('53532'),							
			('53575'),							
			('53531'),							
			('53785'),							
			('53789'),							
			('53793'),							
			('53593'),							
			('53782'),							
			('53590'),							
			('53589'),							
			('53559'),							
			('53560'),							
			('53788')
;
	--select * from @Zips_ALL
	--132
	--select zipcode,count(zipcode) from @Zips_ALL group by ZipCode having count(zipcode) > 1
	--select distinct * from @Zips_ALL
	--132

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
	--7

	--select * from @Zips
	--7

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

	--change existing disaster max end date if applicable
	UPDATE dasforbfed.Disasters
	SET MaxEndDate = EndDate
	WHERE DisasterId = @DisasterID
		AND BeginDate = @BeginDate
		AND Disaster = @Disaster
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