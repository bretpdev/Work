--run on OpsDev
BEGIN TRANSACTION
	DECLARE @ERROR INT = 0,
			@ROWCOUNT INT = 0,
			@EFEND_DATE DATE = CONVERT(DATE,'2021-03-31'), --effective end date of quarter previous to quarter being reported 
			@BEGIN_DATE DATE = CONVERT(DATE,'2021-04-01'), --effective begin date of quarter being reported
			@INSERTS TINYINT = 4; --# of rows to be inserted from step 2
	DECLARE @VAL_CNT TINYINT = 
			(
				SELECT 
					COUNT(*) 
				FROM 
					CSYS..COST_DAT_BatchScriptWeights 
				WHERE 
					EffectiveEnd IS NULL
			) + @INSERTS -- count of rows to update + # being inserted
	;
	--SELECT @BEGIN_DATE,@END_DATE,@VAL_CNT


--STEP 1: UPDATE effective end date
	UPDATE 
		CSYS..COST_DAT_BatchScriptWeights 
	SET 
		EffectiveEnd = @EFEND_DATE 
	WHERE
		EffectiveEnd IS NULL;
    
	-- Save/Set the row count and error number (if any) from the previously executed statement
    SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR;

--STEP 2: INSERT new data
	INSERT INTO
		CSYS..COST_DAT_BatchScriptWeights
		(CostCenterId, [Weight], EffectiveBegin, EffectiveEnd)
	VALUES 
		(4,	3.315168029, @BEGIN_DATE, NULL),
		(8,	91.39418719, @BEGIN_DATE, NULL),
		(13, 0, @BEGIN_DATE, NULL),
		(16,0.170299728, @BEGIN_DATE, NULL),
		(20,5.120345141, @BEGIN_DATE, NULL)
	;
	--5

	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	          
IF @ROWCOUNT = @VAL_CNT AND @ERROR = 0
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

--display data so it can be verified
SELECT * FROM CSYS..COST_DAT_BatchScriptWeights