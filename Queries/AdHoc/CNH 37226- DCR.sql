USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XX
	
DECLARE @Data TABLE(SSN VARCHAR(X), FirstName VARCHAR(XX), LastName VARCHAR(XX))
INSERT INTO @Data
VALUES
('XXXXXXXXX','HENRY','BORGES-RIVERA'),
('XXXXXXXXX','ISSAC','DUNN'),
('XXXXXXXXX','ASHLEY','MACIEL'),
('XXXXXXXXX','JACQUELYNN','ALDERSON'),
('XXXXXXXXX','COREY','CLARK'),
('XXXXXXXXX','KAYLA','GONZALEZ'),
('XXXXXXXXX','CATHERINE','PECKHAM'),
('XXXXXXXXX','JOSHUA','CARLSON'),
('XXXXXXXXX','DOMINIC','BROWN'),
('XXXXXXXXX','CHELSEA','ALVAREZ'),
('XXXXXXXXX','ELLA','CALDWELL-TIBBS'),
('XXXXXXXXX','ASHLEY','WATERMAN'),
('XXXXXXXXX','CLARIBEL','GERMAN'),
('XXXXXXXXX','ANTHONY','ROCHON'),
('XXXXXXXXX','ELIZABETH','ZABKA'),
('XXXXXXXXX','HANNAH','THOMPSON'),
('XXXXXXXXX','ERYKAH','SANTIAGO'),
('XXXXXXXXX','KAREN','PHILLIPS'),
('XXXXXXXXX','CHRISTOPHER','BERLT'),
('XXXXXXXXX','CYNTHIA','MINER'),
('XXXXXXXXX','EVON','KEANE'),
('XXXXXXXXX','GEOFFREY','FAHRINGER'),
('XXXXXXXXX','BENJAMIN','BROOKS'),
('XXXXXXXXX','BETHZAIDA','SOLLA'),
('XXXXXXXXX','FLOYD','ROBINSON'),
('XXXXXXXXX','DOMINIC','TINGLE'),
('XXXXXXXXX','EMILY','STEINMEYER'),
('XXXXXXXXX','ELIZABETH','AMUSAN'),
('XXXXXXXXX','DEREK','PELLETIER'),
('XXXXXXXXX','CONNOR','TEHAN'),
('XXXXXXXXX','JACOB','DWYER'),
('XXXXXXXXX','CHRISTOPHER','SCIBELLI'),
('XXXXXXXXX','CINDY','DAVIS'),
('XXXXXXXXX','CATERA','NOLL')

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR --XX rows

INSERT INTO CLS..ArcAddProcessing(ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, RegardsTo, LN_ATY_SEQ, ProcessingAttempts, CreatedAt, CreatedBy, ProcessedAt)
SELECT DISTINCT
	X,
	PDXX.DF_SPE_ACC_ID,
	'BRTXT',
	'CNH XXXXX',
	GETDATE(),
	'Text Message sent XX/XX/XXXX',
	X,
	X,
	'',
	X,
	X,
	GETDATE(),
	'CNH XXXXX',
	NULL
FROM
	@Data D
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON D.SSN = PDXX.DF_PRS_ID

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR --XX rows



IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
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