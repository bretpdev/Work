--run on UHEAASQLDB

DECLARE @ScriptId VARCHAR(10) = 'CACORRPDF',
		@LetterId INT = 
		(
			SELECT 
				LetterId 
			FROM 
				ULS.[print].Letters 
			WHERE 
				Letter = 'RQRCVDLGP'
		);

DECLARE @ScriptDataIdCoBorrower INT = 
		(
			SELECT 
				ScriptDataId 
			FROM 
				ULS.[print].ScriptData 
			WHERE 
				ScriptID = @ScriptId
				AND LetterId = @LetterId 
				AND IsEndorser = 1
		);

DECLARE @FileHeaderIdCoborrower INT = 
		(
			SELECT
				FileHeaderId
			FROM
				ULS.[print].ScriptData
			WHERE
				ScriptDataId = @ScriptDataIdCoBorrower
		);
--select @ScriptId, @LetterId, @ScriptDataIdCoBorrower,@FileHeaderIdCoborrower --TEST
--select * from ULS.[print].FileHeaders WHERE FileHeaderId = @FileHeaderIdCoborrower --TEST

BEGIN TRANSACTION

	DECLARE @ERROR INT = 0,
			@ROWCOUNT INT = 0
	;

	UPDATE
		ULS.[print].FileHeaders
	SET
		FileHeader = 'KeyLine,Name,Address1,Address2,City,State,ZIP,ForeignState,Country,AccountNumber,CoBorrSSN,CostCenter'
	WHERE
		FileHeader = 'KeyLine,Name,Address1,Address2,City,State,ZIP,ForeignState,Country,AccountNumber,BorrSSN,CostCenter'
		AND CostCenterCodeIndex = 11
		AND FileHeaderId = @FileHeaderIdCoborrower
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