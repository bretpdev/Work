--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 319 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Texas Tropical Storm Imelda (DR-4466)'
				,@BeginDate DATE = CONVERT(DATE,'20190917') --INCIDENT start date (not declaration date!)
				,@AddedBy VARCHAR(50) = 'UNH 63669' --change to current NH ticket
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters);

		--SET IDENTITY_INSERT [dasforbfed].[Disasters] ON --OPSDEV ONLY

			INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
			VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, @AddedBy);--1
			-- Save/Set the row count from the previously executed statement
			SELECT @ROWCOUNT = @@ROWCOUNT;
		
		--SET IDENTITY_INSERT [dasforbfed].[Disasters] OFF --OPSDEV ONLY

		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	
		DECLARE @Zips TABLE (ZipCode VARCHAR(5));
		INSERT INTO @Zips (ZipCode) VALUES
--Chambers	Harris	Jefferson		Liberty		Montgomery	Orange
('77514'),	('77001'),	('77613'),	('77327'),	('77301'),	('77611'),
('77560'),	('77002'),	('77619'),	('77328'),	('77302'),	('77626'),
('77580'),	('77003'),	('77622'),	('77368'),	('77303'),	('77630'),
('77597'),	('77004'),	('77627'),	('77369'),	('77304'),	('77631'),
('77661'),	('77005'),	('77629'),	('77533'),	('77305'),	('77632'),
('77665'),	('77006'),	('77640'),	('77535'),	('77306'),	('77639'),
			('77007'),	('77641'),	('77538'),	('77316'),	('77662'),
			('77008'),	('77642'),	('77561'),	('77318'),	('77670'),
			('77009'),	('77643'),	('77564'),	('77333'),	
			('77010'),	('77651'),	('77575'),	('77353'),	
			('77011'),	('77655'),	('77582'),	('77354'),	
			('77012'),	('77701'),				('77355'),	
			('77013'),	('77702'),				('77356'),	
			('77014'),	('77703'),				('77357'),	
			('77015'),	('77704'),				('77362'),	
			('77016'),	('77705'),				('77365'),	
			('77017'),	('77706'),				('77372'),	
			('77018'),	('77707'),				('77378'),	
			('77019'),	('77708'),				('77380'),	
			('77020'),	('77709'),				('77381'),	
			('77021'),	('77710'),				('77382'),	
			('77022'),	('77713'),				('77384'),	
			('77023'),	('77720'),				('77385'),	
			('77024'),	('77725'),				('77386'),	
			('77025'),	('77726'),				('77387'),	
			('77026'),							('77393'),	
			('77027'),				
			('77028'),				
			('77029'),				
			('77030'),				
			('77031'),				
			('77032'),				
			('77033'),				
			('77034'),				
			('77035'),				
			('77036'),				
			('77037'),				
			('77038'),				
			('77039'),				
			('77040'),				
			('77041'),				
			('77042'),				
			('77043'),				
			('77044'),				
			('77045'),				
			('77046'),				
			('77047'),				
			('77048'),				
			('77049'),				
			('77050'),				
			('77051'),				
			('77052'),				
			('77054'),				
			('77055'),				
			('77056'),				
			('77057'),				
			('77058'),				
			('77059'),				
			('77060'),				
			('77061'),				
			('77062'),				
			('77063'),				
			('77064'),				
			('77065'),				
			('77066'),				
			('77067'),				
			('77068'),				
			('77069'),				
			('77070'),				
			('77071'),				
			('77072'),				
			('77073'),				
			('77074'),				
			('77075'),				
			('77076'),				
			('77077'),				
			('77078'),				
			('77079'),				
			('77080'),				
			('77081'),				
			('77082'),				
			('77083'),				
			('77084'),				
			('77085'),				
			('77086'),				
			('77087'),				
			('77088'),				
			('77089'),				
			('77090'),				
			('77091'),				
			('77092'),				
			('77093'),				
			('77094'),				
			('77095'),				
			('77096'),				
			('77097'),				
			('77098'),				
			('77099'),				
			('77201'),				
			('77202'),				
			('77203'),				
			('77204'),				
			('77205'),				
			('77206'),				
			('77207'),				
			('77208'),				
			('77209'),				
			('77210'),				
			('77212'),				
			('77213'),				
			('77215'),				
			('77216'),				
			('77217'),				
			('77218'),				
			('77219'),				
			('77220'),				
			('77221'),				
			('77222'),				
			('77223'),				
			('77224'),				
			('77225'),				
			('77226'),				
			('77227'),				
			('77228'),				
			('77229'),				
			('77230'),				
			('77231'),				
			('77233'),				
			('77234'),				
			('77235'),				
			('77236'),				
			('77237'),				
			('77238'),				
			('77240'),				
			('77241'),				
			('77242'),				
			('77243'),				
			('77244'),				
			('77245'),				
			('77246'),				
			('77247'),				
			('77248'),				
			('77249'),				
			('77250'),				
			('77251'),				
			('77252'),				
			('77253'),				
			('77254'),				
			('77255'),				
			('77256'),				
			('77257'),				
			('77258'),				
			('77259'),				
			('77260'),				
			('77261'),				
			('77262'),				
			('77263'),				
			('77265'),				
			('77266'),				
			('77267'),				
			('77268'),				
			('77269'),				
			('77270'),				
			('77271'),				
			('77272'),				
			('77273'),				
			('77274'),				
			('77275'),				
			('77276'),				
			('77277'),				
			('77278'),				
			('77279'),				
			('77280'),				
			('77282'),				
			('77284'),				
			('77285'),				
			('77286'),				
			('77287'),				
			('77288'),				
			('77289'),				
			('77290'),				
			('77291'),				
			('77292'),				
			('77293'),				
			('77294'),				
			('77296'),				
			('77297'),				
			('77298'),				
			('77299'),				
			('77315'),				
			('77325'),				
			('77336'),				
			('77337'),				
			('77338'),				
			('77339'),				
			('77345'),				
			('77346'),				
			('77347'),				
			('77373'),				
			('77375'),				
			('77377'),				
			('77379'),				
			('77383'),				
			('77388'),				
			('77389'),				
			('77391'),				
			('77396'),				
			('77401'),				
			('77402'),				
			('77410'),				
			('77411'),				
			('77413'),				
			('77429'),				
			('77433'),				
			('77447'),				
			('77449'),				
			('77450'),				
			('77484'),				
			('77491'),				
			('77492'),				
			('77493'),				
			('77501'),				
			('77502'),				
			('77503'),				
			('77504'),				
			('77505'),				
			('77506'),				
			('77507'),				
			('77508'),				
			('77520'),				
			('77521'),				
			('77522'),				
			('77530'),				
			('77532'),				
			('77536'),				
			('77547'),				
			('77562'),				
			('77571'),				
			('77572'),				
			('77586'),				
			('77587'),				
			('77598')
		;
	--select count(ZipCode) as all_zips from @Zips
	--318
	--select count(distinct ZipCode) as distinct_zips from @Zips
	--318

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
			INSERT INTO 
				[dasforbfed].[Zips]	(ZipCode, DisasterId)
			SELECT DISTINCT
				ZipCode
				,@DisasterID 
			FROM 
				@Zips
		END;

		-- Save/Set the row count from the previously executed statement
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT;

	IF @ROWCOUNT = @ExpectedRowCount
		BEGIN
			PRINT 'Transaction committed.'
			COMMIT TRANSACTION
			--ROLLBACK TRANSACTION
		END
	ELSE
		BEGIN
			PRINT 'Transaction NOT committed.';
			PRINT 'Expected row count not met. Expecting ' +  CAST(@ExpectedRowCount AS VARCHAR(10)) + ' rows, but returned ' + CAST(@ROWCOUNT AS VARCHAR(10))+ ' rows.';
			ROLLBACK TRANSACTION;
		END
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed. Errors found in SQL statement.';
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
