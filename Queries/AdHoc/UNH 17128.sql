BEGIN TRAN
GO

--Select the database to use
USE [BSYS]
GO

DECLARE @Row  float
DECLARE @NumberShouldBeAffected  int

--Change this number if you know how many should be affected
SET @NumberShouldBeAffected = 184

UPDATE dbo.PRNT_DAT_Print
SET ActualPrintedTime = '08/20/2013', PrintDate = '08/20/2013'
WHERE SeqNum BETWEEN 223695 AND 223878

SET @Row = @@ROWCOUNT

--This should never change
IF @Row <> @NumberShouldBeAffected
	BEGIN
		ROLLBACK
		PRINT 'Looking for ' + CAST(@NumberShouldBeAffected AS VARCHAR(5)) + ' rows. Rolled Back because there are ' + CAST(@Row as varchar(5)) + ' rows affected'
	END
ELSE
	BEGIN
		COMMIT
		PRINT 'Committed ' + CAST(@Row AS VARCHAR(5)) + ' rows'
	END