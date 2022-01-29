--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 89

	DECLARE @Disaster VARCHAR(100) = 'Hawaii Severe Storms, Flooding, Landslides, and Mudslides'
	DECLARE @BeginDate DATE = '05/08/2018'

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
		('96701'),
		('96706'),
		('96707'),
		('96709'),
		('96712'),
		('96717'),
		('96730'),
		('96731'),
		('96734'),
		('96744'),
		('96759'),
		('96762'),
		('96782'),
		('96786'),
		('96789'),
		('96791'),
		('96792'),
		('96795'),
		('96797'),
		('96801'),
		('96802'),
		('96803'),
		('96804'),
		('96805'),
		('96806'),
		('96807'),
		('96808'),
		('96809'),
		('96810'),
		('96811'),
		('96812'),
		('96813'),
		('96814'),
		('96815'),
		('96816'),
		('96817'),
		('96818'),
		('96819'),
		('96820'),
		('96821'),
		('96822'),
		('96823'),
		('96824'),
		('96825'),
		('96826'),
		('96827'),
		('96828'),
		('96830'),
		('96835'),
		('96836'),
		('96837'),
		('96838'),
		('96839'),
		('96840'),
		('96841'),
		('96843'),
		('96844'),
		('96846'),
		('96847'),
		('96848'),
		('96849'),
		('96850'),
		('96853'),
		('96854'),
		('96857'),
		('96858'),
		('96859'),
		('96860'),
		('96861'),
		('96863'),
		('96898'),
		('96703'),
		('96705'),
		('96714'),
		('96715'),
		('96716'),
		('96722'),
		('96741'),
		('96746'),
		('96747'),
		('96751'),
		('96752'),
		('96754'),
		('96756'),
		('96765'),
		('96766'),
		('96769'),
		('96796')
	;

	--select  * from @Zips order by ZipCode
--88

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