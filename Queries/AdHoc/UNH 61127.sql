--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0;
	DECLARE @ROWCOUNT INT = 0;
	DECLARE @ExpectedRowCount INT = 28;
	DECLARE @Disaster VARCHAR(100) = 'Iowa Severe Storms and Flooding DR 4421';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Fremont	Harrison	Mills	Monona  Pottawattamie	Shelby		Woodbury
('51650'),('51579'),('51551'),('51572'),('51503'),		('51446'),('51048'),
('51652'),('51557'),('51554'),('51558'),('51549'),		('51593'),('51044'),
('51653'),('51546'),('51561'),('51051'),('51548'),		('51578'),('51052'),
('51639'),('51563'),('51541'),('51040'),('51542'),		('51447'),('51055'),
('51648'),('51556'),('51534'),('51060'),('51553'),		('51537'),('51054'),
('51645'),('51529'),('51533'),('51010'),('51575'),		('51565'),('51039'),
('51654'),('51564'),('51540'),('51063'),('51576'),		('51562'),('51018'),
('51649'),('51545'),('51571'),('51034'),('51559'),		('51531'),('51016'),
('51640'),('51550'),		  ('51523'),('51560'),		('51527'),('51019'),
		  ('51555'),					('51521'),		('51570'),('51030'),
										('51510'),		('51530'),('51026'),
										('51577'),				  ('51106'),
										('51525'),				  ('51105'),
										('51536'),				  ('51108'),
										('51501'),				  ('51111'),
										('51526'),				  ('51109'),
										('51502'),				  ('51104'),
																  ('51004'),
																  ('51056'),
																  ('51101'),
																  ('51103'),
																  ('51102'),
																  ('51015'),
																  ('51007')

	;
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > 1
	--0 hit
	--select * from @Zips_ALL
	--88
	--select distinct * from @Zips_ALL
	--88

	--remove zips that already exist in database
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode)
	SELECT DISTINCT
		ZA.ZipCode
	FROM
		@Zips_ALL ZA --all zip codes from spreadsheet
		LEFT JOIN [dasforbfed].[Zips] DZ --zips in database
			ON ZA.ZipCode = DZ.ZipCode
	WHERE
		DZ.ZipCode IS NULL --removes zips in database
	--28

	--select * from @Zips --new zips to add @ExpectedRowCount
	--28

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

	UPDATE dasforbfed.Disasters
	SET Disaster = 'Iowa Severe Storms and Flooding (DR-4421)'
	WHERE Disaster = 'Iowa Severe Storms and Flooding DR 4421'

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