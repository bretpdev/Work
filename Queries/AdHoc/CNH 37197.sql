BEGIN TRY
	BEGIN TRANSACTION;
		
		INSERT INTO CLS..DirectorEmailParameters
		(
			 DelinquencyLowerLimit
			,DelinquencyUpperLimit
			,MaxEmails
			,AddedAt
			,AddedBy
		)
		VALUES(XXX, XXX, XXXX, GETDATE(), 'CNH XXXXX');

		PRINT 'Transaction committed';

	COMMIT TRANSACTION;
	--ROLLBACK TRANSACTION;
END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION;
	PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.';
	THROW;
END CATCH;