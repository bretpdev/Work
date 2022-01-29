BEGIN TRANSACTION
	DECLARE @END_DATE DATE = CONVERT(DATE,'2021-03-31'), -- effective end date (the last day of the quarter previous to the quarter being reported)
			@BEGIN_DATE DATE = CONVERT(DATE,'2021-04-01'); --effective begin date (the first day of the quarter being reporting)
	--select @BEGIN_DATE,@END_DATE;

	DECLARE @INSERTS INT = 21; --# of rows to be inserted from step 2
	DECLARE @VAL_CNT INT = (
								SELECT COUNT(*) 
								FROM CSYS..COST_DAT_AgentWeights 
								WHERE EffectiveEnd IS NULL
							) + @INSERTS; --NULL count + # of inserts into AgentWeights
	--select @VAL_CNT;

	DECLARE @ERROR INT = 0, 
			@ROWCOUNT INT = 0;

--STEP 1: UPDATE effective end date
	UPDATE CSYS..COST_DAT_AgentWeights 
	SET EffectiveEnd = @END_DATE 
	WHERE EffectiveEnd IS NULL;
	--20

    -- Save/Set the row count and error number (if any) from the previously executed statement
    SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR;
		
--STEP 2: INSERT new data
	INSERT INTO CSYS.[dbo].[COST_DAT_AgentWeights] 
		([SqlUserId], [Weight], [EffectiveBegin], [EffectiveEnd])
	VALUES 
		(1298,0.04,@BEGIN_DATE,NULL)--Bigelow, Riley
		,(1198,0.04,@BEGIN_DATE,NULL)--Blair, Jeremy
		,(1399,0.04,@BEGIN_DATE,NULL)--Cole, Candice
		,(1395,0.08,@BEGIN_DATE,NULL)--Garfield, Melanie
		,(4311,0.03,@BEGIN_DATE,NULL) --Green, Chad
		,(1686,0.04,@BEGIN_DATE,NULL)--Gregory, Savanna
		,(1256,0.04,@BEGIN_DATE,NULL)--Gutierrez, Jesse
		,(1152,0.05,@BEGIN_DATE,NULL)--Hack, Wendy
		,(1519,0.04,@BEGIN_DATE,NULL)--Halladay, David
		,(1518,0.04,@BEGIN_DATE,NULL)--Hanson, Jessica
		,(1820,0.04,@BEGIN_DATE,NULL)--Kieschnick, Jared
		,(4179,0.05,@BEGIN_DATE,NULL)--Kramer, Jacob
		,(1703,0.04,@BEGIN_DATE,NULL)--MacDonald, Conor
		,(1268,0.05,@BEGIN_DATE,NULL)--McComb, Colton
		,(1570,0.04,@BEGIN_DATE,NULL)--Ostler, Steven
		,(1276,0.06,@BEGIN_DATE,NULL)--Pehrson, Bret
		,(1161,0.09,@BEGIN_DATE,NULL)--Phillips, Debbie
		,(1280,0.08,@BEGIN_DATE,NULL)--Ryan, Jarom
		,(1451,0.07,@BEGIN_DATE,NULL)--Walker, D. Evan
		,(3996,0.03,@BEGIN_DATE,NULL)--Westerman, Karleann
		,(1573,0.06,@BEGIN_DATE,NULL)--Wright, Joshua
		;
	   -- Update the row count and error number (if any) from the previously executed statement
       SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	          
IF @ROWCOUNT = @VAL_CNT AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed';
		COMMIT TRANSACTION;
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10));
		PRINT 'EXPECTED ROWCOUNT: ' + CAST(@INSERTS AS VARCHAR(10));
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10));
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.';
		ROLLBACK TRANSACTION;
	END