CREATE PROCEDURE [dbo].[Lt20MarkLetterPrinted]
	@LetterSeq int,
	@LetterId varchar(10),
	@AccountNumber varchar(10)

AS BEGIN
	BEGIN TRANSACTION
	UPDATE 
		LT20_LetterRequests
	SET 
		PrintedAt = GETDATE()
	WHERE 
		RN_SEQ_LTR_CRT_PRC = @LetterSeq
		AND
		RM_DSC_LTR_PRC = @LetterId
		and 
		DF_SPE_ACC_ID = @AccountNumber
		and 
		PrintedAt is null

	IF @@ERROR = 0
		BEGIN
			COMMIT TRANSACTION
		END
	ELSE
		BEGIN
			RAISERROR ('Failed to retrieve dbo.DocumentDetails record for processing.', -- error message
				   16, -- Severity.
				   1 -- State.
				   );
			ROLLBACK TRANSACTION
		END
	END
