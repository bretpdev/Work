--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 286 --# of zips + 1 disaster forb

	DECLARE @Disaster VARCHAR(100) = 'North Carolina Hurricane Florence (DR-4393)'
	DECLARE @BeginDate DATE = '09/14/2018'

	----OPSDEV:
	--INSERT INTO [dasforbfed].[Disasters] (Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
	--VALUES(@Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 179, @BeginDate), 1, 'UNH 58362')

	--LIVE:
	DECLARE @DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters)
	INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
	VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 179, @BeginDate), 1, 'UNH 58362')
	--1

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	
	--Affected zip codes:
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
	
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode) VALUES
--North Carolina Hurricane Florence Zip Codes																										
--Beaufort	Bladen		Brunswick	Columbus	Carteret	Craven		Cumberland	Duplin		Harnett		Hoke 		Hyde		Johnston	Jones		Lee			Lenoir		Moore		New Hanover	Onslow		Pamlico		Pender		pitt		Richmond	Robeson		Sampson		Scotland	Wayne 		Wilson
('27810'),	('28320'),	('28469'),	('28463'),	('28584'),	('28519'),	('28356'),	('28508'),	('28334'),	('28376'),	('27875'),	('27504'),	('28585'),	('28355'),	('28525'),	('28394'),	('28404'),	('28540'),	('28509'),	('28457'),	('27884'),	('28345'),	('28375'),	('28393'),	('28396'),	('27530'),	('27896'),
('27817'),	('28433'),	('28470'),	('28438'),	('28584'),	('28527'),	('28301'),	('28518'),	('28368'),				('27960'),	('27577'),	('28573'),	('27331'),	('28504'),	('28370'),	('28449'),	('28539'),	('28587'),	('28478'),	('27836'),	('28338'),	('28377'),	('28318'),	('28353'),	('27533'),	('27894'),
('27821'),	('28337'),	('28467'),	('28432'),	('28524'),	('28562'),	('28348'),	('28466'),	('27546'),				('27885'),	('27542'),	('28555'),	('27330'),	('28572'),	('28387'),	('28429'),	('28542'),	('28537'),	('28435'),	('27828'),	('28330'),	('28371'),	('28444'),	('28352'),	('28365'),	('27895'),
('27814'),	('28332'),	('28462'),	('28472'),	('28511'),	('28564'),	('28303'),	('28453'),	('28339'),				('27826'),	('27528'),	('28522'),	('27237'),	('28551'),	('28315'),	('28403'),	('28541'),	('28552'),	('28425'),	('27834'),	('28379'),	('28372'),	('28366'),	('28343'),	('27531'),	('27822'),
('27889'),	('28399'),	('28465'),	('28430'),	('28520'),	('28560'),	('28302'),	('28458'),	('27501'),				('27824'),	('27593'),				('27505'),	('28501'),	('28373'),	('28480'),	('28545'),	('28556'),	('28421'),	('27827'),	('28380'),	('28384'),	('28328'),	('28351'),	('27830'),	('27880'),
('27806'),	('28448'),				('28436'),	('28512'),	('28586'),	('28312'),	('28398'),	('28335'),							('27568'),				('27332'),	('28503'),	('28374'),	('28409'),	('28546'),	('28583'),	('28454'),	('27835'),	('28367'),	('28386'),	('28329'),				('27534'),	('27873'),
('27865'),	('28434'),				('28455'),	('28516'),	('28532'),	('28342'),	('28341'),	('27521'),							('27569'),							('28502'),	('27242'),	('28401'),	('28574'),	('28571'),	('28443'),	('27812'),	('28347'),	('28378'),	('28447'),				('27863'),	('27851'),
('27860'),	('28392'),				('28456'),	('28557'),	('28526'),	('28311'),	('28464'),	('27506'),							('27555'),										('27259'),	('28408'),	('28547'),	('28510'),				('27858'),	('28363'),	('28383'),	('28441'),				('28578'),	('27813'),
('27808'),							('28442'),	('28570'),	('28523'),	('28314'),	('28349'),	('27543'),							('27576'),										('28350'),	('28402'),	('28544'),	('28529'),				('27879'),				('28369'),	('28385'),				('27532'),	('27893'),
									('28439'),	('28553'),	('28533'),	('28310'),	('28325'),	('28323'),							('27527'),										('28327'),	('28428'),	('28584'),	('28515'),				('28530'),				('28357'),	('28382'),				('28333'),	('27883'),
									('28431'),	('28528'),	('28563'),	('28331'),	('28521'),	('27552'),							('27524'),										('28388'),	('28412'),	('28543'),							('27837'),				('28358'),	('28344'),			
									('28424'),	('28531'),	('28561'),	('28395'),	('28424'),	('28326'),							('27520'),										('27376'),	('28407'),	('28460'),							('28513'),				('28319'),				
									('28423'),							('28305'),	('28423'),																						('27281'),	('28405'),	('28445'),							('27811'),				('28340'),				
									('28450'),							('28309'),	('28450'),																						('27325'),	('28406'),										('27829'),				('28362'),				
																		('28306'),																									('28530'),	('28411'),										('28590'),				('28364'),				
																		('28308'),																									('27837'),	('28410'),										('27833'),				('28359'),				
																		('28390'),																									('28513'),																			('28360'),				
																		('28307'),																									('27811'),											
																		('28304'),																									('27829'),											
																		('28391'),																									('28590'),											
																																													('27833')
;

--select * from @Zips order by ZipCode
--285

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