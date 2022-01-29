USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @Expected INT = X

UPDATE CLS.docid.DocumentTypes SET DocumentType = 'Bankruptcy - CHXX MOC' WHERE DocumentTypesId = XX

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

INSERT INTO CLS.docid.DocumentTypes(DocumentType)
VALUES
('Bankruptcy - CHX MOC'),
('Bankruptcy - BANK CONCL')

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

INSERT INTO CLS.docid.Documents(Document)
VALUES
('CRMCX'),
('CRMCC')

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

INSERT INTO CLS.docid.Arcs(Arc)
VALUES
('CRMCX'),
('CRMCC')

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

DECLARE @DocumentId INT = (SELECT DocumentsId FROM CLS.docid.Documents WHERE Document = 'CRMCX')
DECLARE @DocumentTypeId INT = (SELECT DocumentTypesId FROM CLS.docid.DocumentTypes WHERE DocumentType = 'Bankruptcy - CHX MOC')
DECLARE @ArcId INT = (SELECT ArcId FROM CLS.docid.Arcs WHERE Arc = 'CRMCX')

INSERT INTO CLS.docid.DocIdMapping(DocumentId, DocumentTypeId, ArcId)
VALUES(@DocumentId, @DocumentTypeId, @ArcId)
	
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


DECLARE @DocumentIdX INT = (SELECT DocumentsId FROM CLS.docid.Documents WHERE Document = 'CRMCC')
DECLARE @DocumentTypeIdX INT = (SELECT DocumentTypesId FROM CLS.docid.DocumentTypes WHERE DocumentType = 'Bankruptcy - BANK CONCL')
DECLARE @ArcIdX INT = (SELECT ArcId FROM CLS.docid.Arcs WHERE Arc = 'CRMCC')

INSERT INTO CLS.docid.DocIdMapping(DocumentId, DocumentTypeId, ArcId)
VALUES(@DocumentIdX, @DocumentTypeIdX, @ArcIdX)
	
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @Expected AND @ERROR = X
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