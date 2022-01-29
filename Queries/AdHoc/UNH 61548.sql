--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0;
	DECLARE @ROWCOUNT INT = 0;
	DECLARE @ExpectedRowCount INT = 29;
	DECLARE @Disaster VARCHAR(100) = 'Iowa Severe Storms and Flooding (DR-4421)';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Fremont	Harrison	Louisa		Mills		Monona	Pottawattamie	Scott		Shelby		Woodbury
('51639'),	('51529'),	('52640'),	('51533'),	('51010'),('51501'),	('52722'),	('51446'),	('51004'),
('51640'),	('51545'),	('52646'),	('51534'),	('51034'),('51502'),	('52726'),	('51447'),	('51007'),
('51645'),	('51546'),	('52653'),	('51540'),	('51040'),('51503'),	('52728'),	('51527'),	('51015'),
('51648'),	('51550'),	('52737'),	('51541'),	('51051'),('51510'),	('52745'),	('51530'),	('51016'),
('51649'),	('51555'),	('52738'),	('51551'),	('51060'),('51521'),	('52746'),	('51531'),	('51018'),
('51650'),	('51556'),	('52752'),	('51554'),	('51063'),('51525'),	('52748'),	('51537'),	('51019'),
('51652'),	('51557'),	('52754'),	('51561'),	('51523'),('51526'),	('52753'),	('51562'),	('51026'),
('51653'),	('51563'),				('51571'),	('51558'),('51536'),	('52756'),	('51565'),	('51030'),
('51654'),	('51564'),							('51572'),('51542'),	('52758'),	('51578'),	('51039'),
			('51579'),									  ('51548'),	('52765'),	('51593'),	('51044'),
														  ('51549'),	('52767'),				('51048'),
														  ('51553'),	('52768'),				('51052'),
														  ('51559'),	('52773'),				('51054'),
														  ('51560'),	('52801'),				('51055'),
														  ('51570'),	('52802'),				('51056'),
														  ('51575'),	('52803'),				('51101'),
														  ('51576'),	('52804'),				('51102'),
														  ('51577'),	('52805'),				('51103'),
																		('52806'),				('51104'),
																		('52807'),				('51105'),
																		('52808'),				('51106'),
																		('52809'),				('51108'),
																								('51109'),
																								('51111')
	;
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > 1
	--0 hit
	--select * from @Zips_ALL
	--117
	--select distinct * from @Zips_ALL
	--117

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
	--29

	--select * from @Zips --new zips to add @ExpectedRowCount
	--29

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