USE ECorrFed
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

INSERT INTO ECorrFed.dbo.Letters(Letter,LetterTypeId,DocId,Viewable,ReportDescription,ReportName,Viewed,MainframeRegion,SubjectLine,DocSource,DocComment,WorkFlow,DocDelete,Active)
VALUES('TSXXBSPLIT',X,'XELT','Y','Description','Name','N','VUKX','Transfer of Your Loan Account','IMPORT','Transfer of Your Loan Account','N','N',X),
('TSXXBPRTCH',X,'XELT','Y','Description','Name','N','VUKX','Transfer of Your Loan Account','IMPORT','Transfer of Your Loan Account','N','N',X),
('TSXXBPSTFR',X,'XELT','Y','Description','Name','N','VUKX','Transfer of Your Loan Account','IMPORT','Transfer of Your Loan Account','N','N',X),
('TSXXBTRNCL',X,'XELT','Y','Description','Name','N','VUKX','Transfer of Your Loan Account','IMPORT','Transfer of Your Loan Account','N','N',X)

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

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
