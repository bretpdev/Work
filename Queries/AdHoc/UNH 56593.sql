--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 58
	DECLARE @DisasterID INT = (SELECT MAX(DisasterId) + 1 FROM dasforbfed.Disasters)
	DECLARE @Disaster VARCHAR(100) = 'North Carolina Tornado and Severe Storms'
	DECLARE @BeginDate DATE = '05/08/2018'
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode) VALUES
		('27420'),
		('27419'),
		('27406'),
		('27301'),
		('27407'),
		('27495'),
		('27455'),
		('27405'),
		('27402'),
		('27401'),
		('27377'),
		('27403'),
		('27425'),
		('27499'),
		('27404'),
		('27417'),
		('27427'),
		('27429'),
		('27413'),
		('27416'),
		('27415'),
		('27264'),
		('27283'),
		('27282'),
		('27410'),
		('27408'),
		('27409'),
		('27435'),
		('27412'),
		('27438'),
		('27411'),
		('27310'),
		('27265'),
		('27342'),
		('27497'),
		('27268'),
		('27498'),
		('27261'),
		('27214'),
		('27262'),
		('27263'),
		('27313'),
		('27357'),
		('27235'),
		('27358'),
		('27260'),
		('27249'),
		('27233'),
		('27288'),
		('27289'),
		('27320'),
		('27375'),
		('27326'),
		('27323'),
		('27048'),
		('27025'),
		('27027')

--select  * from @Zips order by ZipCode
--57
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