--run on UHEAASQLDB
USE CLS;
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X;
	DECLARE @ROWCOUNT INT = X;
	DECLARE @ExpectedRowCount INT = XXX;
	DECLARE @Disaster VARCHAR(XXX) = 'Oklahoma Severe Storms, Straight-line Winds, Tornadoes, And Flooding (DR-XXXX)';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(X),DisasterID INT);
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
--Canadian	Cherokee  Creek		Delaware  Kay	  Le Flore	Logan	   Mayes	  Noble	    Nowata	Okmulgee	Osage	Ottawa	   Payne  Pottawatomie	Rogers	Sequoyah	Washington
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),			('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),					('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),							  ('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
				    ('XXXXX'),		    ('XXXXX'),('XXXXX'),		  ('XXXXX'),							  ('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),		  ('XXXXX')
												 ,('XXXXX'),		  ('XXXXX'),							  ('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX')
												 ,('XXXXX'),		  ('XXXXX'),										('XXXXX'),			('XXXXX')		
												 ,('XXXXX'),		  ('XXXXX')										
												 ,('XXXXX')												
												 ,('XXXXX')												
												 ,('XXXXX')												
												 ,('XXXXX')												
												 ,('XXXXX')												
												 ,('XXXXX')
	;

	--adds disaster id to zip code list
	UPDATE @ZIPS_ALL
	SET DISASTERID = @DISASTERID;

	--select * from @Zips_ALL;
	--XXX

	----check for dupes:
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > X
	----X hit
	--select * from @Zips_ALL
	----XXX
	--select distinct * from @Zips_ALL
	----XXX


	--already exist in database
	--SELECT ZipCode,DisasterID FROM @Zips_ALL
	--INTERSECT
	--SELECT ZipCode,DisasterId FROM dasforbfed.Zips WHERE DisasterId = @DisasterID
	--XX

	--don't exist in database that we want to add
	DECLARE @Zips TABLE (ZipCode VARCHAR(X))
	INSERT INTO @Zips (ZipCode)
	SELECT DISTINCT ZipCode
	FROM
	(
		SELECT ZipCode,DisasterID FROM @Zips_ALL
		EXCEPT
		SELECT ZipCode,DisasterId FROM dasforbfed.Zips WHERE DisasterId = @DisasterID
	)EX
	--XXX

	--select * from @Zips
	--XXX

	IF NOT EXISTS 
	(
		SELECT 
			ZipId 
		FROM 
			[dasforbfed].[Zips] ZX
			INNER JOIN @Zips ZX
				ON ZX.ZipCode = ZX.ZipCode
		WHERE
			ZX.DisasterId = @DisasterID
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

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END