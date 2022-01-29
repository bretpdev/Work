--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 52
	DECLARE @Disaster VARCHAR(100) = 'Nebraska Severe Winter Storm -DR4420'
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate)
	--select @Disaster,@DisasterID,@BeginDate

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Boone 	Buffalo		Custer		Knox 	Richardson	Santee Indian Resevation 	Thurston 
('68655'),	('68861'), ('68855'), ('68783'), ('68442'), ('68718'),					('68047'),
('68660'),	('68858'), ('68814'), ('68789'), ('68437'), 							('68039'),
('68623'),	('68866'), ('68828'), ('68760'), ('68457'), 							('68062'),
('68627'),	('68876'), ('68813'), ('68729'), ('68376'), 							('68071'),
('68652'),	('68870'), ('69120'), ('68730'), ('68433'), 							('68055'),
('68620'),	('68869'), ('68822'), ('68718'), ('68355'), 							('68067'),
			('68845'), ('68825'), ('68724'), ('68431'), 		
			('68847'), ('68860'), ('68786'), ('68337'), 		
			('68848'), ('68856'), 				
			('68812'), ('68881'), 				
			('68836'), ('68874'), 				
			('68840'),				
			('68849')				
	;
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > 1
	--1 hit
	--select * from @Zips_ALL
	--53
	--select distinct * from @Zips_ALL
	--52

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
	--52

	--select * from @Zips
	--52

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

	UPDATE dasforbfed.Disasters
	SET Disaster = 'Nebraska Severe Winter Storm (DR4420)'
	WHERE Disaster = 'Nebraska Severe Winter Storm -DR4420'

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