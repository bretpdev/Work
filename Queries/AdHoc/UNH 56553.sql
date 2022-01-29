--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 136
	DECLARE @DisasterID INT = 6
	DECLARE @Disaster VARCHAR(100) = 'Indiana Severe Storms and Flooding'
	DECLARE @BeginDate DATE = '05/05/2018'
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode) VALUES
	--Carroll	Clark		Elkhart		Floyd		Harrison	Jefferson	Lake		Marshall	St. Joseph
	('46913'),	('47104'),	('46507'),	('47119'),	('47107'),	('47224'),	('46303'),	('46501'),	('46530'),
	('46915'),	('47106'),	('46514'),	('47122'),	('47110'),	('47230'),	('46307'),	('46504'),	('46536'),
	('46916'),	('47111'),	('46515'),	('47124'),	('47112'),	('47231'),	('46308'),	('46506'),	('46544'),
	('46917'),	('47126'),	('46516'),	('47146'),	('47114'),	('47243'),	('46311'),	('46511'),	('46545'),
	('46920'),	('47129'),	('46517'),	('47150'),	('47115'),	('47250'),	('46312'),	('46513'),	('46546'),
	('46923'),	('47130'),	('46526'),	('47151'),	('47117'),				('46319'),	('46537'),	('46552'),
	('46929'),	('47131'),	('46527'),				('47135'),				('46320'),	('46563'),	('46554'),
	('46977'),	('47132'),	('46528'),				('47136'),				('46321'),	('46570'),	('46556'),
	('47997'),	('47133'),	('46540'),				('47142'),				('46322'),	('46572'),	('46561'),
				('47134'),	('46543'),				('47160'),				('46323'),				('46574'),
				('47141'),	('46550'),				('47161'),				('46324'),				('46595'),
				('47143'),	('46553'),				('47164'),				('46325'),				('46601'),
				('47144'),	('46573'),				('47166'),				('46327'),				('46604'),
				('47147'),													('46342'),				('46613'),
				('47162'),													('46355'),				('46614'),
				('47163'),													('46356'),				('46615'),
				('47172'),													('46373'),				('46616'),
				('47190'),													('46375'),				('46617'),
				('47199'),													('46376'),				('46619'),
																			('46377'),				('46620'),
																			('46394'),				('46624'),
																			('46401'),				('46626'),
																			('46402'),				('46628'),
																			('46403'),				('46634'),
																			('46404'),				('46635'),
																			('46405'),				('46637'),
																			('46406'),				('46660'),
																			('46407'),				('46680'),
																			('46408'),				('46699'),
																			('46409'),		
																			('46410'),		
																			('46411')		

--select  * from @Zips order by ZipCode
--135
	;
	INSERT INTO [dasforbfed].[Disasters]
		(DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active)
	VALUES(@DisasterID, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 179, @BeginDate), 1)
	--1

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

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
	--135

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