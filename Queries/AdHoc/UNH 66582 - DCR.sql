USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @Expected INT = 8

INSERT INTO BSYS..GENR_REF_AuthAccess (TypeKey, WinUName)
VALUES
('Key ID Agent','dcox'),
('Key ID Agent','dblair'),
('Key ID Agent','dsomers'),
('Key ID Agent','kboswell'),
('Key ID Agent','sriddle'),
('Key ID Agent','rlloyd'),
('Key ID Agent','asansone'),
('Key ID Agent','eadams')
-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

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