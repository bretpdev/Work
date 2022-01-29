USE CompleteFinancialFafsa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @Expected INT = 5

INSERT INTO CompleteFinancialFafsa.compfafsa.Schools(SchoolId, DistrictId, SchoolName, StreetAddress, City, AddedAt, AddedBy, DeletedAt, DeletedBy)
VALUES('490018201467',NULL,'Lumen Scholar Institute','1353 W 760 N','Orem',GETDATE(),SUSER_SNAME(),NULL,NULL)

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

UPDATE 
	[CompleteFinancialFafsa].[compfafsa].[SchoolVariations]
SET
	AdjustedSchoolId = '490018201467',
	AdjustedAt = GETDATE(),
	AdjustedBy = SUSER_SNAME()
  where SchoolName like ('LUMEN%')
-- Save/Set the row count and error number (if any) from the previously executed statement

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @Expected AND @ERROR = 0
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