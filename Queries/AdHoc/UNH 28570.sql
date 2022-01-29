USE EmailTracking
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

UPDATE 
	EG
SET
	EG.Recipient = 'sburke@utahsbr.edu',
	Copied1 = 'sburke@utahsbr.edu',
	Copied2 = 'sburke@utahsbr.edu',
	Copied3 = 'alin@utahsbr.edu, sshoemaker@utahsbr.edu, zcavanaugh@utahsbr.edu, mmckinney@utahsbr.edu',
	Copied4 = 'Nothing',
	Copied5 = 'Nothing'
FROM 
	[EmailTracking].[dbo].[EmailGroups] EG
WHERE
	EG.Number = 9

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 1 AND @ERROR = 0
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
