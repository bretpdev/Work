--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 53

	DECLARE @Disaster VARCHAR(100) = 'Texas Severe Storms and Flooding'
	DECLARE @BeginDate DATE = '07/06/2018'

	DECLARE @DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters)

	INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active)
	VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 179, @BeginDate), 1)
	--1

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	
	--Affected zip codes:
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
	
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode) VALUES
		('78502'), ('78568'),
		('78501'), ('78597'),
		('78503'), ('78586'),
		('78572'), ('78583'),
		('78573'), ('78578'),
		('78542'), ('78593'),
		('78576'), ('78575'),
		('78577'), ('78592'),
		('78574'), ('78523'),
		('78549'), ('78526'),
		('78543'), ('78535'),
		('78504'), ('78520'),
		('78599'), ('78521'),
		('78579'), ('78522'),
		('78560'), ('78550'),
		('78558'), ('78559'),
		('78538'), ('78566'),
		('78589'), ('78567'),
		('78537'), ('78551'),
		('78557'), ('78552'),
		('78562'), ('78553'),
		('78570'),
		('78595'),
		('78505'),
		('78516'),
		('78565'),
		('78563'),
		('78596'),
		('78539'),
		('78540'),
		('78541')
	;

--select  * from @Zips order by ZipCode
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