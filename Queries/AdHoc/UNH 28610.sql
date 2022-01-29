USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO cpp.FileTypes(FileType)
	VALUES('Comma'),
	('Direct'),
	('Flat')

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO cpp.PaymentSources(PaymentSource, InstitutionId, [FileName], FileTypeId, Active, CreatedAt ,CreatedBy)
	VALUES('Direct', '919191', 'DirectLoansConsol', 2, 1, GETDATE(), 'DCR'),
	('Great Lakes', 'GRTDIR', 'GLConsol', 3, 1, GETDATE(), 'DCR'),
	('Nelnet', 'NELDIR', 'NelnetConsol', 3, 1, GETDATE(), 'DCR'),
	('PHEAA', 'FLSDIR', 'PheaaConsol', 3, 1, GETDATE(), 'DCR'),
	('Sallie Mae', 'SLMDIR', 'SLMAConsol', 3, 1, GETDATE(), 'DCR')

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 8 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
