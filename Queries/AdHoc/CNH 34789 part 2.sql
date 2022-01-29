--run on UHEAASQLDB
USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXX

	UPDATE
		PP
	SET 
		PP.PrintedAt = NULL,
		PP.EcorrDocumentCreatedAt = NULL,
		PP.ImagedAt = NULL
	FROM
		CLS.[print].letters L
		INNER JOIN CLS.[print].ScriptData SD
			ON SD.LetterId = L.LetterId
		INNER JOIN CLS.[print].PrintProcessing PP
			ON PP.ScriptDataId = SD.ScriptDataId
			AND DATEDIFF(DAY,PrintedAt,'XXXX-XX-XX') = X
			AND PrintedAt > 'XXXX-XX-XX XX:XX:XX.XXX'
	WHERE
		L.Letter = 'RPDISCFED'

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
