BEGIN TRY
	BEGIN TRANSACTION
		INSERT INTO 
			CLS..ManagerEmailParameters
			(
				 DelinquencyLowerLimit
				,DelinquencyUpperLimit
				,MaxEmails
				,AddedAt
				,AddedBy
			)
		VALUES
			(
				 XX
				,XX
				,XXXXX
				,GETDATE()
				,'CNH XXXXX'
			)
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	PRINT 'CNH XXXXX.sql transaction NOT committed.';
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;