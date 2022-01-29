USE master

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	--570
	DELETE FROM
		TAU
	--SELECT COUNT(*)
	FROM
		NeedHelpCornerStone.[dbo].[DAT_TicketsAssociatedUserID] TAU
		INNER JOIN CSYS..SYSA_DAT_Users SDU ON SDU.SqlUserId = TAU.SqlUserId
	WHERE
		SDU.LastName = 'Riding'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	--1501
	DELETE FROM
		ER
	--SELECT COUNT(*)
	FROM
		NeedHelpCornerStone..REF_EmailRecipient ER
		INNER JOIN CSYS..SYSA_DAT_Users SDU ON SDU.SqlUserId = ER.SqlUserId
	WHERE
		SDU.LastName = 'Riding'

	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	--480
	DELETE FROM
		TAU
	--SELECT COUNT(*)
	FROM
		NeedHelpUheaa.[dbo].[DAT_TicketsAssociatedUserID] TAU
		INNER JOIN CSYS..SYSA_DAT_Users SDU ON SDU.SqlUserId = TAU.SqlUserId
	WHERE
		SDU.LastName = 'Riding'
	
	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	--1860
	DELETE FROM
		ER
	--SELECT COUNT(*)
	FROM
		NeedHelpUheaa..REF_EmailRecipient ER
		INNER JOIN CSYS..SYSA_DAT_Users SDU ON SDU.SqlUserId = ER.SqlUserId
	WHERE
		SDU.LastName = 'Riding'

		-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 5041 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
