USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @DisasterId INT = (SELECT MAX(DisasterId) FROM dasforbfed.Disasters) + 1
	DECLARE @BeginDate DATETIME = '05/11/2018'

	INSERT INTO dasforbfed.Disasters(DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active)
	VALUES(@DisasterId, 'Hawaii Kilauea Volcanic Eruption and Earthquakes', @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 179, @BeginDate), 1)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO dasforbfed.Zips(ZipCode, DisasterId)
	VALUES('96704', @DisasterId),
		('96710', @DisasterId),
		('96718', @DisasterId),
		('96719', @DisasterId),
		('96720', @DisasterId),
		('96721', @DisasterId),
		('96725', @DisasterId),
		('96726', @DisasterId),
		('96727', @DisasterId),
		('96728', @DisasterId),
		('96737', @DisasterId),
		('96738', @DisasterId),
		('96739', @DisasterId),
		('96740', @DisasterId),
		('96743', @DisasterId),
		('96745', @DisasterId),
		('96749', @DisasterId),
		('96750', @DisasterId),
		('96755', @DisasterId),
		('96760', @DisasterId),
		('96764', @DisasterId),
		('96771', @DisasterId),
		('96772', @DisasterId),
		('96773', @DisasterId),
		('96774', @DisasterId),
		('96776', @DisasterId),
		('96777', @DisasterId),
		('96778', @DisasterId),
		('96780', @DisasterId),
		('96781', @DisasterId),
		('96783', @DisasterId),
		('96785', @DisasterId)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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