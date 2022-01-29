USE CLS
GO

BEGIN TRANSACTION

DECLARE @ERROR INT = X
DECLARE @ROWCOUNT INT = X,
		@ArcId int,
		@DocumentTypeId int,
		@DocumentId int

INSERT INTO cls.docid.Arcs(Arc)
VALUES('DIFRD')

SET @ArcId = @@IDENTITY
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

INSERT INTO cls.docid.DocumentTypes(DocumentType)
VALUES('Fed Deconvertions ')

SET @DocumentTypeId = @@IDENTITY
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

INSERT INTO cls.docid.Documents(Document)
VALUES('CRFRD')

SET @DocumentId = @@IDENTITY
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

INSERT INTO cls.docid.DocIdMapping(ArcId, DocumentId, DocumentTypeId)
VALUES(@ArcId, @DocumentId, @DocumentTypeId)

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