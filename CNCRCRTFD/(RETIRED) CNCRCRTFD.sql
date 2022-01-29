USE CDW;
GO

BEGIN TRY
	BEGIN TRANSACTION

	DECLARE @NewData TABLE 
		(
			DF_SPE_ACC_ID VARCHAR(10)
			,BF_SSN VARCHAR(9)
			,LN_SEQ SMALLINT
		);
	DECLARE @ArcLoan TABLE
		(
			ArcAddProcessingId INT
			,AccountNumber VARCHAR(10)
		);
	DECLARE @ARC VARCHAR(5) = 'CTDXP', --ARC to write
			@ScriptId VARCHAR(10) = 'CNCRCRTFD',
			@NOW DATETIME = GETDATE();
	DECLARE	@TODAY DATE = @NOW;

	--select @ARC,@ScriptId,@NOW,@TODAY --TEST

	/**** insert deferments and forbearance accounts ***/
	INSERT INTO @NewData 
	(
		DF_SPE_ACC_ID,
		BF_SSN,
		LN_SEQ
	)
	--deferment
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID
		,LN50.BF_SSN
		,LN50.LN_SEQ
		----test fields:
		--,LN50.LD_DFR_END
		--,EXCLUDE_ARC.LD_ATY_REQ_RCV
		--,@TODAY as today
		--,DF10.LC_DFR_TYP
		--,DF10.LC_DFR_SUB_TYP
	FROM
		LN10_LON LN10
		INNER JOIN PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN LN50_BR_DFR_APV LN50
			ON LN10.BF_SSN = LN50.BF_SSN
			AND LN10.LN_SEQ = LN50.LN_SEQ
		INNER JOIN DF10_BR_DFR_REQ DF10
			ON LN50.BF_SSN = DF10.BF_SSN
			AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
		LEFT JOIN
		(--EXCLUDE: already have arc during last month of deferment
			SELECT
				AY10.BF_SSN
				,LN85.LN_SEQ
				,CONVERT(DATE,AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				AY10_BR_LON_ATY AY10
				INNER JOIN LN85_LON_ATY LN85
					ON AY10.BF_SSN = LN85.BF_SSN
					AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
			WHERE
				AY10.PF_REQ_ACT = @ARC
				AND AY10.LC_STA_ACTY10 = 'A'
		) EXCLUDE_ARC
			ON LN10.BF_SSN = EXCLUDE_ARC.BF_SSN
			AND LN10.LN_SEQ = EXCLUDE_ARC.LN_SEQ
			AND EXCLUDE_ARC.LD_ATY_REQ_RCV >= DATEADD(MONTH,-1,LN50.LD_DFR_END) -- use forb attribute when updating that portion of the query
			AND EXCLUDE_ARC.LD_ATY_REQ_RCV <= LN50.LD_DFR_END
	WHERE
		EXCLUDE_ARC.BF_SSN IS NULL
		AND LN10.LA_CUR_PRI > 0.00
		AND LN10.LC_STA_LON10 = 'R'
		AND LN50.LC_STA_LON50 = 'A'
		AND DF10.LC_DFR_STA = 'A'
		AND DF10.LC_STA_DFR10 = 'A'
		AND LN50.LC_DFR_RSP != '003' --exclude denied deferments
		AND DATEADD(MONTH,-1,LN50.LD_DFR_END) <= @TODAY
		AND LN50.LD_DFR_END >= @TODAY
		AND DF10.LC_DFR_TYP = '37'
		AND DF10.LC_DFR_SUB_TYP IN ('CP','CT')
				
	UNION

	--forbearance
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID
		,LN60.BF_SSN
		,LN60.LN_SEQ
		----test fields:
		--,LN60.LD_FOR_END
		--,EXCLUDE_ARC.LD_ATY_REQ_RCV
		--,@TODAY as today
		--,FB10.LC_FOR_TYP
		--,FB10.LC_FOR_SUB_TYP
	FROM
		LN10_LON LN10
		INNER JOIN PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN LN60_BR_FOR_APV LN60
			ON LN10.BF_SSN = LN60.BF_SSN
			AND LN10.LN_SEQ = LN60.LN_SEQ
		INNER JOIN FB10_BR_FOR_REQ FB10
			ON LN60.BF_SSN = FB10.BF_SSN
			AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
		LEFT JOIN
		(--EXCLUDE: already have arc during last month of deferment
			SELECT
				AY10.BF_SSN
				,LN85.LN_SEQ
				,CONVERT(DATE,AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				AY10_BR_LON_ATY AY10
				INNER JOIN LN85_LON_ATY LN85
					ON AY10.BF_SSN = LN85.BF_SSN
					AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
			WHERE
				AY10.PF_REQ_ACT = @ARC
				AND AY10.LC_STA_ACTY10 = 'A'
		) EXCLUDE_ARC
			ON LN10.BF_SSN = EXCLUDE_ARC.BF_SSN
			AND LN10.LN_SEQ = EXCLUDE_ARC.LN_SEQ
			AND EXCLUDE_ARC.LD_ATY_REQ_RCV >= DATEADD(MONTH,-1,LN60.LD_FOR_END) -- use forb attribute when updating that portion of the query
			AND EXCLUDE_ARC.LD_ATY_REQ_RCV <= LN60.LD_FOR_END
	WHERE
		EXCLUDE_ARC.BF_SSN IS NULL
		AND LN10.LA_CUR_PRI > 0.00
		AND LN10.LC_STA_LON10 = 'R'
		AND LN60.LC_STA_LON60 = 'A'
		AND FB10.LC_FOR_STA = 'A'
		AND FB10.LC_STA_FOR10 = 'A'
		AND LN60.LC_FOR_RSP != '003' --exclude denied deferments
		AND DATEADD(MONTH,-1,LN60.LD_FOR_END) <= @TODAY
		AND LN60.LD_FOR_END >= @TODAY
		AND FB10.LC_FOR_TYP = '34'
	;
	--select * from @NewData --TEST

	/*** add records into AAP ***/
	INSERT INTO CLS..ArcAddProcessing (ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessingAttempts, CreatedAt, CreatedBy)
	OUTPUT INSERTED.ArcAddProcessingId, INSERTED.AccountNumber INTO @ArcLoan(ArcAddProcessingId, AccountNumber)
	SELECT DISTINCT
		0 AS ArcTypeId, --Atd22ByLoan (Add arc by sequence number)
		NewData.DF_SPE_ACC_ID AS AccountNumber,
		@ARC AS ARC,
		@ScriptId AS ScriptId,
		@NOW AS ProcessOn,
		'Cancer Recertification notice' AS Comment,
		0 AS IsReference,
		0 AS IsEndorser,
		0 AS ProcessingAttempts,
		@NOW AS CreatedAt,
		SUSER_SNAME() AS CreatedBy
	FROM
		@NewData NewData
		LEFT JOIN CLS..ArcAddProcessing ExistingAAP
			ON ExistingAAP.AccountNumber = NewData.DF_SPE_ACC_ID
			AND ExistingAAP.ARC = @ARC
			AND ExistingAAP.ScriptId = @ScriptId
			AND CONVERT(DATE,ExistingAAP.CreatedAt) = CONVERT(DATE,@NOW)
	WHERE
		ExistingAAP.AccountNumber IS NULL --No matching existing record
	;

	/*** add record into AAP Loan Selection ***/
	INSERT INTO CLS..ArcLoanSequenceSelection (ArcAddProcessingId, LoanSequence)
	SELECT
		AL.ArcAddProcessingId
		,NewData.LN_SEQ
	FROM
		@NewData NewData
		INNER JOIN @ArcLoan AL
			ON NewData.DF_SPE_ACC_ID = AL.AccountNumber
	;

	DELETE FROM @ArcLoan; --clear out ArcLoan to use it again

	COMMIT TRANSACTION;
END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT,
			@ProcessNotificationId INT,
			@NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'), --Error report
			@NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@NOW,@NOW,@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;

--select * from cls..ArcAddProcessing where ScriptId = 'CNCRCRTFD';--for testing only