BEGIN TRAN
GO

--Select the database to use
USE BSYS
GO

DECLARE @Row  float
DECLARE @NumberShouldBeAffected  int
DECLARE @PrintDate date

--Change this number if you know how many should be affected
SET @NumberShouldBeAffected = 226
SET @PrintDate = '2013-08-21'

--SQL Statement goes here
UPDATE 
	dbo.PRNT_DAT_Print
SET
	[PrintDate] = NULL,
    [ActualPrintedTime] = NULL
WHERE
	CAST(PrintDate as DATE) = @PrintDate
	
--This must be the first thing after the sql statement
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