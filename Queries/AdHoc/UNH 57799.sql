USE ULS
GO

BEGIN TRANSACTION

	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @DisasterId INT = (SELECT MAX(DisasterId) FROM dasforbfed.Disasters) + 1
	DECLARE @BeginDate DATETIME = '08/04/2018'

INSERT INTO dasforbfed.Disasters(DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active)
VALUES(@DisasterId, 'California Wildfires And High Winds', @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 179, @BeginDate), 1)

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR --1

INSERT INTO dasforbfed.Zips(ZipCode, DisasterId)
VALUES
('96040', @DisasterId),
('96007', @DisasterId),
('96008', @DisasterId),
('96047', @DisasterId),
('96049', @DisasterId),
('96051', @DisasterId),
('96016', @DisasterId),
('96022', @DisasterId),
('96001', @DisasterId),
('96011', @DisasterId),
('96002', @DisasterId),
('96028', @DisasterId),
('96003', @DisasterId),
('96017', @DisasterId),
('96013', @DisasterId),
('96033', @DisasterId),
('96062', @DisasterId),
('96087', @DisasterId),
('96019', @DisasterId),
('96089', @DisasterId),
('96079', @DisasterId),
('96084', @DisasterId),
('96076', @DisasterId),
('96073', @DisasterId),
('96095', @DisasterId),
('96070', @DisasterId),
('96069', @DisasterId),
('96065', @DisasterId),
('96071', @DisasterId),
('96096', @DisasterId),
('96088', @DisasterId),
('96099', @DisasterId)

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR --32

IF @ROWCOUNT = 33 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END