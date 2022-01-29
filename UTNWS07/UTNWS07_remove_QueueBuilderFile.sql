--run on UHEAASQLDB
BEGIN TRY
	BEGIN TRANSACTION;
		--select * from cls..QueueBuilderFile where FileName like 'UNWS07%'
		DELETE FROM 
			CLS..QueueBuilderFile 
		WHERE
			[FileName] LIKE 'UNWS07%'
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH;
	ROLLBACK TRANSACTION;
	PRINT 'Transaction NOT committed';
	THROW;
END CATCH;