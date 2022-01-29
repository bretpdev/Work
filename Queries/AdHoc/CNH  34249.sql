USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	DELETE FROM LTDB_Letter_Header_Mapping
	WHERE LetterHeaderMappingId IN (XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX)

		-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	DECLARE @ForeignState INT = (SELECT HeaderId FROM LTDB_File_Headers WHERE Header = 'FOREIGN STATE')
	DECLARE @LetterId INT = (SELECT DocDetailId FROM LTDB_DAT_DocDetail WHERE ID = 'FMXXBNGRD')
	INSERT INTO LTDB_Letter_Header_Mapping(LetterId, HeaderTypeId, HeaderId, [Order], CreatedBy, CreatedOn, Active)
	VALUES(@LetterId, XX, @ForeignState, X, USER_NAME(), GETDATE(), X)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE
		LTDB_Letter_Header_Mapping
	SET
		[Order] = X
	WHERE
	 LetterHeaderMappingId = XXX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	DELETE SP
	FROM
		LTDB_SystemLettersStoredProcedures SP
		INNER JOIN LTDB_DAT_DocDetail DD
			ON SP.LetterId = DD.DocDetailId
	WHERE
		DD.ID = 'FMXXBNGRD'

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = XX AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END