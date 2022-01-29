USE CLS 
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	DECLARE @Document CHAR(X) = 'CRCTD'
	DECLARE @DocumentId INT
	DECLARE @DocumentType VARCHAR(XXX) = 'Cancer Treatment Deferment Request'
	DECLARE @DocumentTypeId INT
	DECLARE @Arc VARCHAR(X) = 'DICTD'
	DECLARE @ArcId INT

	INSERT INTO CLS.docid.Documents(Document)
	VALUES(@Document)

	SET @DocumentId = SCOPE_IDENTITY()

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO CLS.docid.DocumentTypes(DocumentType)
	VALUES(@DocumentType)

	SET @DocumentTypeId = SCOPE_IDENTITY()

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	INSERT INTO CLS.docid.Arcs(Arc)
	VALUES(@Arc)

	SET @ArcId = SCOPE_IDENTITY()

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	INSERT INTO CLS.docid.DocIdMapping(DocumentId, DocumentTypeId, ArcId)
	VALUES(@DocumentId, @DocumentTypeId, @ArcId)

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
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END