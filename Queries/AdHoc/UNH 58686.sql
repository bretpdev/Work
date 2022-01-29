--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 26 --# of zips + 1 disaster forb

	DECLARE @Disaster VARCHAR(100) = 'Georgia Hurricane Michael (DR-4400)'
	DECLARE @BeginDate DATE = '10/14/2018'
	DECLARE @AddedBy VARCHAR(50) = 'UNH 58686'

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
	--Baker		Decatur		Dougherty	Early		Miller		Seminole
	('39870'),	('39815'),	('31701'),	('39823'),	('39837'),	('39845'),
				('39817'),	('31702'),	('39832'),				('39859'),
				('39818'),	('31703'),	('39841'),		
				('39819'),	('31704'),	('39861'),		
				('39825'),	('31705'),			
				('39834'),	('31706'),			
				('39852'),	('31707'),			
							('31708'),			
							('31721'),			
							('31782')
	;

--select * from @Zips order by ZipCode
--25

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