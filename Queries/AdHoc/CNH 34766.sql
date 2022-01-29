USE CLS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	DECLARE @Arc CHAR(X) = 'CRPDS'
	DECLARE @CS_ID INT

	INSERT INTO CLS.docid.Documents(Document)
	VALUES(@Arc)

	SET @CS_ID = (SELECT SCOPE_IDENTITY())

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	DECLARE @CS_ARC_ID INT

	INSERT INTO CLS.docid.Arcs(Arc)
	VALUES(@Arc)

	SET @CS_ARC_ID = (SELECT SCOPE_IDENTITY())

	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	DECLARE @CS_TYPE_ID INT

	INSERT INTO CLS.docid.DocumentTypes(DocumentType)
	VALUES('Payment Disputes - Cornerstone')

	SET @CS_TYPE_ID = (SELECT SCOPE_IDENTITY())

	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	INSERT INTO CLS.docid.DocIdMapping(DocumentId, DocumentTypeId, ArcId)
	VALUES(@CS_ID, @CS_TYPE_ID, @CS_ARC_ID)

	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = X AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END