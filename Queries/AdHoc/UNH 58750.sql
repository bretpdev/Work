--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 126 --# of zips + 1 disaster forb

	DECLARE @Disaster VARCHAR(100) = 'Wisconsin Severe Storms, Tornadoes, Straight-line Winds, Flooding, And Landslides (DR-4402)'
	DECLARE @BeginDate DATE = '10/18/2018'
	DECLARE @AddedBy VARCHAR(50) = 'UNH 58750'

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
--Crawford	Dane		Juneau		La Crosse	Monroe		Richland	Sauk 		Vernon
('54645'),	('53791'),	('54646'),	('54669'),	('54670'),	('53556'),	('53583'),	('54652'),
('54655'),	('53706'),	('53944'),	('54644'),	('54666'),	('53581'),	('53958'),	('54651'),
('54631'),	('53708'),	('53948'),	('54636'),	('54656'),	('54664'),	('53577'),	('54639'),
('54657'),	('53711'),	('53929'),	('54602'),	('54660'),	('53540'),	('53578'),	('54667'),
('53826'),	('53707'),	('54637'),	('54614'),	('54619'),	('53584'),	('53588'),	('54665'),
('54654'),	('53597'),	('54641'),	('54603'),	('54620'),	('53924'),	('53913'),	('54658'),
('53821'),	('53703'),	('54618'),	('54601'),	('54648'),				('53959'),	('54623'),
('54626'),	('53701'),	('53962'),	('54650'),	('54662'),				('53961'),	('54632'),
('54628'),	('53702'),	('53968'),	('54653'),	('54638'),				('53943'),	('54624'),
			('53598'),	('53950'),				('54649'),				('53942'),	('54634'),
			('53705'),													('53940'),	('54621'),
			('53529'),													('53951'),	
			('53704'),													('53561'),	
			('53725'),													('53941'),	
			('53790'),													('53937'),	
			('53719'),						
			('53718'),						
			('53523'),						
			('53744'),						
			('53508'),						
			('53515'),						
			('53517'),						
			('53726'),						
			('53528'),						
			('53715'),						
			('53714'),						
			('53794'),						
			('53713'),						
			('53783'),						
			('53717'),						
			('53527'),						
			('53716'),						
			('53774'),						
			('53596'),						
			('53562'),						
			('53784'),						
			('53792'),						
			('53558'),						
			('53777'),						
			('53786'),						
			('53571'),						
			('53572'),						
			('53532'),						
			('53575'),						
			('53531'),						
			('53785'),						
			('53789'),						
			('53793'),						
			('53593'),						
			('53782'),						
			('53590'),						
			('53589'),						
			('53559'),						
			('53560'),						
			('53788')
	;

--select * from @Zips order by ZipCode
--125

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