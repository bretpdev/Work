USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X


INSERT INTO CDW.dbo.FormatTranslation(FmtName, Label, Start)
VALUES('$SCHTYP', 'REPAYE ALTERNATIVE','IA'),
('$SCHTYP', 'REVISED PAY AS YOU EARN','IX'),
('$SCHTYP', 'EXIT COUNSELING - REPAYE','RE')

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