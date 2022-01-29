BEGIN TRAN
GO

--Select the database to use
USE [BSYS]
GO

DECLARE @Row  float
DECLARE @NumberShouldBeAffected  int

--Change this number if you know how many should be affected
SET @NumberShouldBeAffected = 9

UPDATE dbo.QCTR_LST_ReportsToProcess
SET BusinessUnit = 'Account Services'
WHERE BusinessUnit = 'Auxiliary Services'
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