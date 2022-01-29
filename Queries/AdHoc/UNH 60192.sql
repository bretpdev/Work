--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 59 --# of distinct zips + 1 disaster forb

	DECLARE @Disaster VARCHAR(100) = 'Alaska Earthquake (DR-4413)'
	DECLARE @BeginDate DATE = '01/31/2019'
	DECLARE @AddedBy VARCHAR(50) = 'UNH 60192'

	DECLARE @DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters)
	INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
	VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, @AddedBy)
	--1

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	
	--Affected zip codes:
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
	
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode) VALUES
--Anchorage	Kenai Peninsula	Matanuska-Susitna 
('99518'),	('99663'),		('99652'),
('99522'),	('99664'),		('99654'),
('99521'),	('99669'),		('99694'),
('99520'),	('99631'),		('99623'),
('99519'),	('99556'),		('99629'),
('99530'),	('99682'),		('99688'),
('99540'),	('99635'),		('99645'),
('99529'),	('99672'),		('99676'),
('99523'),	('99639'),		('99674'),
('99524'),	('99568'),		('99687'),
('99503'),	('99572'),		('99683'),
('99502'),	('99603'),		('99667'),
('99567'),	('99611'),
('99505'),	('99610'),
('99504'),	('99605'),
('99587'),	
('99599'),	
('99695'),	
('99501'),	
('99577'),	
('99506'),	
('99514'),	
('99513'),	
('99515'),	
('99517'),	
('99516'),	
('99508'),	
('99507'),	
('99509'),	
('99511'),	
('99510')
;

--select count(*) as AllZips from @Zips
----58
--select count(distinct ZipCode) as DistinctZips from @Zips
----58
--select ZipCode,count(ZipCode) from @Zips group by ZipCode having count(ZipCode) > 1


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