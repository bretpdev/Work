--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 61 --# of zips + 1 disaster forb

	DECLARE @Disaster VARCHAR(100) = 'South Carolina Hurricane Florence (DR-4394)'
	DECLARE @BeginDate DATE = '09/16/2018'
	DECLARE @AddedBy VARCHAR(50) = 'UNH 58453'

	DECLARE @DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters)
	INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
	VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 179, @BeginDate), 1, @AddedBy)
	--1

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	
	--Affected zip codes:
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
	
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode) VALUES
	--Chesterfield	Darlington	Dillon		Florence	Georgetown	Horry		Marion		Marlboro 
	('29727'),		('29551'),	('29536'),	('29530'),	('29510'),	('29545'),	('29571'),	('29512'),
	('29728'),		('29079'),	('29563'),	('29502'),	('29442'),	('29572'),	('29546'),	('29596'),
	('29741'),		('29550'),	('29547'),	('29501'),				('29569'),	('29592'),	('29594'),
	('29709'),		('29069'),	('29565'),	('29161'),				('29568'),	('29589'),	('29570'),
	('29101'),		('29593'),				('29503'),				('29526'),		
	('29718'),		('29540'),				('29583'),				('29597'),		
					('29532'),				('29506'),				('29511'),		
											('29591'),				('29544'),		
											('29114'),				('29528'),		
											('29505'),				('29527'),		
											('29555'),				('29575'),		
											('29541'),				('29581'),		
											('29560'),				('29579'),		
											('29504'),				('29578'),		
																	('29588'),		
																	('29587'),		
																	('29582'),		
																	('29576'),		
																	('29577')
	;

--select * from @Zips order by ZipCode
--61

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