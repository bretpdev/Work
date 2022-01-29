--run on UHEAASQLDB
DECLARE @ScriptId VARCHAR(10) = 'SYSAPPDEFF',
		@LetterId INT = 
		(
			SELECT
				LetterId 
			FROM 
				ULS.[print].Letters 
			WHERE
				Letter = 'SADEFER'
		);

DECLARE @ScriptDataId INT =
		(
			SELECT 
				ScriptDataId 
			FROM 
				ULS.[print].ScriptData 
			WHERE 
				ScriptID = @ScriptId
				AND LetterId = @LetterId 
		)
--select @TODAY,@ScriptId,@LetterId,@FiveDaysAgo,@ScriptDataId --test

BEGIN TRANSACTION

	DECLARE @ERROR INT = 0,
			@ROWCOUNT INT = 0
	;

	UPDATE 
		ULS.[print].ScriptData
	SET 
		SourceFile = NULL,
		CheckForCoBorrower = 0
	WHERE
		ScriptDataId = @ScriptDataId
		AND SourceFile = 'ULWO31.LWO31R2.*'
	;
	--1
	
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR --1

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