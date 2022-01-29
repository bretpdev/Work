--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 76 --# of zips + 1 disaster forb

	DECLARE @Disaster VARCHAR(100) = 'Florida Hurricane Michael (DR-4399)'
	DECLARE @BeginDate DATE = '10/11/2018'
	DECLARE @AddedBy VARCHAR(50) = 'UNH 58670'

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
--Bay		Calhoun		Franklin	Gadsden		Gulf		Holmes		Jackson		Liberty		Taylor		Wakulla		Washington
('32444'),	('32449'),	('32424'),	('32352'),	('32465'),	('32455'),	('32445'),	('32334'),	('32357'),	('32358'),	('32427'),
('32461'),	('32421'),	('32329'),	('32330'),	('32457'),	('32464'),	('32443'),	('32321'),	('32347'),	('32355'),	('32437'),
('32412'),	('32430'),	('32323'),	('32353'),	('32456'),	('32425'),	('32446'),	('32360'),	('32348'),	('32327'),	('32428'),
('32438'),				('32320'),	('32343'),				('32452'),	('32460'),	('32335'),	('32356'),	('32326'),	('32462'),
('32410'),				('32322'),	('32333'),							('32448'),				('32359'),	('32346'),	('32463'),
('32411'),							('32351'),							('32447'),				
('32401'),							('32332'),							('32442'),				
('32466'),							('32324'),							('32426'),				
('32461'),																('32423'),				
('32461'),																('32420'),				
('32405'),																('32440'),				
('32407'),																('32432'),				
('32403'),																('32431'),				
('32404'),										
('32408'),										
('32409'),										
('32406'),										
('32402'),										
('32413'),										
('32417')
;

--select * from @Zips order by ZipCode
--75

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