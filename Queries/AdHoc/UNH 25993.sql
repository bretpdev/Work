USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		ArcAddProcessing
	SET
		ProcessedAt = null
	WHERE
		ArcAddProcessingId IN (988191,988343,988486,988629,988767,988197,988346,988494,988636,988775,988204,988354,988502,988640,988779,988080,988215,988365,988513,988651,988792,988075,988211,988359,988506,988647,988786,988085,988225,988370,988520,988659,988799,988098,988240,988391,988537,988675,988818,988094,988236,988385,988532,988670,988811,988108,988256,988402,988548,988687,988829,988089,988226,988378,988526,988664,988805,988103,988247,988396,988543,988681,988822,988118,988267,988413,988559,988698,988125,988273,988420,988565,988705,988135,988284,988432,988577,988716,988140,988290,988440,988583,988722,988129,988279,988427,988571,988710,988163,988317,988466,988605,988745,988147,988300,988445,988588,988727,988184,988334,988481,988624,988763,988171,988323,988467,988612,988751,988151,988302,988453,988594,988733,988158,988311,988456,988601,988739,988112,988259,988409,988554,988694,988180,988331,988477,988616,988756)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	
	IF @ROWCOUNT = 128 AND @ERROR = 0
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
GO


USE NobleCalls
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		NobleCalls.dbo.NobleCallHistory
	SET
		Deleted = 1,
		DeletedBy = 'DCR',
		DeletedAt = GETDATE()
	WHERE
		NobleCallHistoryId = 1442962

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
