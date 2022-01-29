BEGIN TRAN
GO

--Select the database to use
USE CSYS
GO

DECLARE @Row  float
DECLARE @NumberShouldBeAffected  int

--Change this number if you know how many should be affected
SELECT
	 @NumberShouldBeAffected = COUNT(*)
FROM
	 dbo.SYSA_DAT_Users
WHERE 
	BusinessUnit = 10
	
PRINT @NumberShouldBeAffected

--SQL Statement goes here
UPDATE
	dbo.SYSA_DAT_Users
SET
	BusinessUnit = 3
WHERE
	BusinessUnit = 10

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