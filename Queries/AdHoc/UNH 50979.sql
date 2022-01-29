BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 3

--Query Body

UPDATE
	[EcorrUheaa].[dbo].[DocumentDetails]
SET
	[Path] = REPLACE([Path], 'X:\PADD\Central Printing\Ecorr\', '\bulk\FILENET_UHEAA_UT\InboundRequest\{0}\'),
	Printed = NULL
WHERE 
	NOT [Path] LIKE '\bulk\FILENET_UHEAA_UT\%'

--Query Body

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
