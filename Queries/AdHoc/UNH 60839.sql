USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 61

INSERT INTO ULS.dasforbfed.Disasters(DisasterId, Disaster, BeginDate, EndDate, [Days], MaxEndDate, MaxDays, Active, AddedAt, AddedBy)
VALUES(22,'Iowa Severe Storms and Flooding DR 4421','2019-03-23','2019-06-20',90,'2019-06-20',90,1,GETDATE(),'UNH 60839')

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR --1 rows

INSERT INTO ULS.dasforbfed.Zips(ZipCode, DisasterId)
VALUES('51650',22),('51652',22),('51653',22),('51639',22),('51648',22),('51645',22),('51654',22),('51649',22),('51640',22),('51579',22),('51557',22),('51546',22),('51563',22),('51556',22),('51529',22),('51564',22),('51545',22),('51550',22),('51555',22),('51551',22),('51554',22),('51561',22),('51541',22),('51534',22),('51533',22),('51540',22),('51571',22),('51572',22),('51558',22),('51051',22),('51040',22),('51060',22),('51010',22),('51063',22),('51034',22),('51523',22),('51048',22),('51044',22),('51052',22),('51055',22),('51054',22),('51039',22),('51018',22),('51016',22),('51019',22),('51030',22),('51026',22),('51106',22),('51105',22),('51108',22),('51111',22),('51109',22),('51104',22),('51004',22),('51056',22),('51101',22),('51103',22),('51102',22),('51015',22),('51007',22)

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR --60 rows

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END