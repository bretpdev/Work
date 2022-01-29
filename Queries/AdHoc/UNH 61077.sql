--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 66
	DECLARE @Disaster VARCHAR(100) = 'Nebraska Severe Winter Storm (DR4420)'
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate)
	--select @Disaster,@DisasterID,@BeginDate

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Antelope	Boone 	  Boyd 		Buffalo	  Burt		Butler	 Cass		Cuming	 Custer		Colfax	 Dodge		Douglas	 Hall		Howard	  Knox 		Madison 	Nance	Pierce	 Platte	  Richardson  Saline	Nemaha	  Santee Indian Resevation 	Sarpy 	  Saunders	Stanton	  Thurston 	Washington 
('68764'),('68655'),('68755'),('68861'),('68020'),('68626'),('68307'),('68788'),('68855'),('68661'),('68044'),('68164'),('68803'),('68820'),('68783'),('68752'),('68640'),('68747'),('68634'),('68442'),('68453'),('68305'),('68718'),					('68138'),('68018'),('68768'),('68047'),('68029'),
('68773'),('68660'),('68746'),('68858'),('68019'),('68632'),('68304'),('68716'),('68814'),('68629'),('68025'),('68172'),('68883'),('68872'),('68789'),('68781'),('68638'),('68738'),('68631'),('68437'),('68445'),('68321'),							('68136'),('68042'),('68779'),('68039'),('68068'),
('68761'),('68623'),('68777'),('68866'),('68045'),('68624'),('68455'),('68004'),('68828'),('68641'),('68621'),('68175'),('68810'),('68873'),('68760'),('68701'),		  ('68765'),('68653'),('68457'),('68465'),('68379'),							('68147'),('68033'),		  ('68062'),('68034'),
('68756'),('68627'),('68719'),('68876'),('68061'),('68001'),('68058'),('68791'),('68813'),('68659'),('68057'),('68154'),('68802'),('68835'),('68729'),('68715'),		  ('68769'),('68601'),('68376'),('68464'),('68320'),							('68005'),('68648'),		  ('68071'),('68023'),
('68720'),('68652'),('68722'),('68870'),('68038'),('68014'),('68413'),			('69120'),('68643'),('68633'),('68106'),('68832'),('68831'),('68730'),('68702'),		  ('68767'),('68602'),('68433'),('68333'),('68414'),							('68157'),('68041'),		  ('68055'),('68002'),
('68636'),('68620')			 ,('68869')			 ,('68036'),('68349'),			('68822')		   ,('68072'),('68107'),('68801'),('68838'),('68718'),('68758'),				    ('68642'),('68355'),('68341'),('68421'),							('68133'),('68040'),		  ('68067'),('68008'),
('68726')					 ,('68845')			 ,('68669'),('68016'),			('68825')		   ,('68063'),('68105'),('68824'),			('68724'),('68748'),				    ('68647'),('68431'),('68343'),('68378'),							('68059'),('68070'),			
							  ('68847')			 ,('68658'),('68407'),			('68860')		   ,('68664'),('68103'),					('68786'),							    ('68644'),('68337'),('68359'),										('68056'),('68066'),			
							  ('68848')			 ,('68635'),('68048'),			('68856')		   ,('68026'),('68104'),																																('68113'),('68073'),			
							  ('68812')			 ,('68667'),('68409'),			('68881')		   ,('68649'),('68108'),																																('68123'),('68003'),			
							  ('68836')					   ,('68463'),			('68874')		   ,('68031'),('68112'),																																('68128'),('68017'),			
							  ('68840')					   ,('68403')										 ,('68114'),																																('68028'),('68050'),			
							  ('68849')					   ,('68037')										 ,('68111'),																																('68046'),('68065'),			
														    ('68366')										 ,('68109'),																																		  ('68015'),			
														    ('68347')										 ,('68110')																
																											 ,('68182')																
																											 ,('68007')																
																											 ,('68183')																
																											 ,('68198')																
																											 ,('68197')																
																											 ,('68010')																
																											 ,('68101')																
																											 ,('68102')																
																											 ,('68069')																
																											 ,('68022')																
																											 ,('68064')																
																											 ,('68180')																
																											 ,('68142')																
																											 ,('68139')																
																											 ,('68135')																
																											 ,('68137')																
																											 ,('68144')																
																											 ,('68152')																
																											 ,('68176')																
																											 ,('68145')																
																											 ,('68179')																
																											 ,('68178')																
																											 ,('68119')																
																											 ,('68122')																
																											 ,('68118')																
																											 ,('68116')																
																											 ,('68117')																
																											 ,('68124')																
																											 ,('68132')																
																											 ,('68134')																
																											 ,('68131')																
																											 ,('68127')																
																											 ,('68130')																
				
	;
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > 1
	--1 hit
	--select * from @Zips_ALL
	--248
	--select distinct * from @Zips_ALL
	--247

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
	--66

	--select * from @Zips
	--66

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
	SET Disaster = 'Nebraska Severe Winter Storm (DR-4420)'
	WHERE Disaster = 'Nebraska Severe Winter Storm (DR4420)'

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