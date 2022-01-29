DECLARE @ARC VARCHAR(5) = 'PLUSY';
DECLARE @SCRIPTID VARCHAR(10) = 'DEQPLENRO';
DECLARE @COMMENT VARCHAR(300) = 'PLUS loan(s) delinquent with HT or more enrollment.';
DECLARE @LOANTYPE VARCHAR(8) = 'DLPLUS';
DECLARE @ArcAddProcessingId INT;

DECLARE @ArcAddProcessingIds TABLE(ArcAddProcessingId INT, AccountNumber VARCHAR(10));
DECLARE @Pop TABLE(AccountNumber VARCHAR(10), LoanSequence SMALLINT);

INSERT INTO @Pop(AccountNumber, LoanSequence)
SELECT DISTINCT
	DF_SPE_ACC_ID,
	LN_SEQ
FROM
(
	--enrollment status changed to enrolled after a delinquency occured
	SELECT DISTINCT 
		PD10.DF_SPE_ACC_ID,
		LN10.LN_SEQ
	FROM
		CDW..LN10_LON LN10
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN CDW..SD10_STU_SPR SD10
			ON LN10.LF_STU_SSN = SD10.LF_STU_SSN
	--LN16_MAX_OCC delinquent loans and most recent delinquency occured date for the loan
		INNER JOIN
		(
			SELECT
				LN16.BF_SSN,
				LN16.LN_SEQ,
				MAX(LN16.LD_DLQ_OCC) AS LD_DLQ_OCC
			FROM
				CDW..LN16_LON_DLQ_HST LN16
			WHERE
				LN16.LC_STA_LON16 = '1'
				AND LN16.LC_DLQ_TYP = 'P'
				AND 
				(
					LN16.LD_DLQ_MAX >= DATEADD(DAY,-5,CAST(GETDATE() AS DATE)) --Account for AES holidays that we dont honor
				)
			GROUP BY
				LN16.BF_SSN,
				LN16.LN_SEQ
		) LN16_MAX_OCC
			ON LN10.BF_SSN = LN16_MAX_OCC.BF_SSN
			AND LN10.LN_SEQ = LN16_MAX_OCC.LN_SEQ
	--WQ20_L select PD01 queue tasks for exclusion
		LEFT JOIN 
		(
			SELECT
				WQ20.BF_SSN,
				LN85.LN_SEQ
			FROM
				CDW..WQ20_TSK_QUE WQ20
				INNER JOIN CDW..LN85_LON_ATY LN85
					ON WQ20.BF_SSN = LN85.BF_SSN
					AND WQ20.LN_ATY_SEQ = LN85.LN_ATY_SEQ
			WHERE
				WQ20.WF_QUE = 'PD'
				AND WQ20.WF_SUB_QUE = '01'
				AND WQ20.WC_STA_WQUE20 NOT IN ('C', 'X') --Status is not closed or cancelled
		) WQ20_L
			ON LN10.BF_SSN = WQ20_L.BF_SSN
			AND LN10.LN_SEQ = WQ20_L.LN_SEQ
	--new DLPLUS loans after the loan that is delinquent that have the same student and have the field Deferment was requested on application = Y
		LEFT JOIN CDW..LN10_LON NEW
			ON LN10.LD_LON_1_DSB < NEW.LD_LON_1_DSB
			AND NEW.IC_LON_PGM = @LOANTYPE
			AND LN10.LF_STU_SSN = NEW.LF_STU_SSN
			AND NEW.LI_DFR_REQ_ON_APL = 'Y'
			AND NEW.LA_CUR_PRI > 0.00
			AND NEW.LC_STA_LON10 = 'R'
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND SD10.LC_STA_STU10 = 'A'
		AND LN10.LA_CUR_PRI > 0.00
		AND LN10.IC_LON_PGM = @LOANTYPE
		AND SD10.LC_REA_SCL_SPR IN ('10', '11', '19') --10 ENROLLED HALF TIME, 11 ENROLLED FULL TIME, 19 ENROLLED 3QTR TIME
		AND SD10.LD_SCL_SPR >= CAST(LN16_MAX_OCC.LD_DLQ_OCC AS DATE)
		AND SD10.LD_ENR_STA_EFF_CAM > LN16_MAX_OCC.LD_DLQ_OCC --enrollment status changed to enrolled after a delinquency occured
		AND (LN10.LI_DFR_REQ_ON_APL = 'Y'  OR NEW.BF_SSN IS NOT NULL) --Deferment was requested on application OR there are new DLPLUS loans after the loan that is delinquent
		AND WQ20_L.BF_SSN IS NULL

	UNION ALL

	--Borrowers with enrollment information arc
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID,
		LN10.LN_SEQ
	FROM
		CDW..PD10_PRS_NME PD10
		INNER JOIN CDW..LN10_LON LN10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN CDW..LN16_LON_DLQ_HST LN16
			ON LN10.BF_SSN = LN16.BF_SSN
			AND LN10.LN_SEQ = LN16.LN_SEQ
		INNER JOIN CDW..AY10_BR_LON_ATY AY10
			ON LN10.BF_SSN = AY10.BF_SSN
		INNER JOIN CDW..AY15_ATY_CMT AY15
			ON AY10.BF_SSN = AY15.BF_SSN
			AND AY10.LN_ATY_SEQ = AY15.LN_ATY_SEQ
			AND LC_STA_AY15 = 'A'
		INNER JOIN CDW..AY20_ATY_TXT AY20
			ON AY15.BF_SSN = AY20.BF_SSN
			AND AY15.LN_ATY_SEQ = AY20.LN_ATY_SEQ
			AND AY15.LN_ATY_CMT_SEQ = AY20.LN_ATY_CMT_SEQ
		LEFT JOIN --Excluding borrowers with non-closed PD01 tasks
		(
			SELECT
				WQ20.BF_SSN,
				LN85.LN_SEQ
			FROM
				CDW..WQ20_TSK_QUE WQ20
				INNER JOIN CDW..LN85_LON_ATY LN85
					ON WQ20.BF_SSN = LN85.BF_SSN
					AND WQ20.LN_ATY_SEQ = LN85.LN_ATY_SEQ
			WHERE
				WQ20.WF_QUE = 'PD'
				AND WQ20.WF_SUB_QUE = '01'
				AND WQ20.WC_STA_WQUE20 NOT IN ('C', 'X') --Status is not closed or cancelled
		) WQ20_L
			ON LN10.BF_SSN = WQ20_L.BF_SSN
			AND LN10.LN_SEQ = WQ20_L.LN_SEQ
	--new DLPLUS loans after the loan that is delinquent that have the same student and have the field Deferment was requested on application = Y
		LEFT JOIN CDW..LN10_LON NEW
			ON LN10.LD_LON_1_DSB < NEW.LD_LON_1_DSB
			AND NEW.IC_LON_PGM = @LOANTYPE
			AND LN10.LF_STU_SSN = NEW.LF_STU_SSN
			AND NEW.LI_DFR_REQ_ON_APL = 'Y'
			AND NEW.LA_CUR_PRI > 0.00
			AND NEW.LC_STA_LON10 = 'R'
	WHERE
		LN10.IC_LON_PGM = @LOANTYPE
		AND LN10.LA_CUR_PRI > 0.00
		AND LN10.LC_STA_LON10 = 'R'
		AND LN16.LC_STA_LON16 = '1'
		AND LN16.LC_DLQ_TYP = 'P'
		AND LN16.LN_DLQ_MAX > 0
		AND (LN10.LI_DFR_REQ_ON_APL = 'Y' OR NEW.BF_SSN IS NOT NULL) --Deferment was requested on application OR there are new DLPLUS loans after the loan that is delinquent
		AND AY10.PF_REQ_ACT = 'PS544'
		AND AY10.LD_ATY_RSP > LN16.LD_DLQ_OCC
		AND WQ20_L.BF_SSN IS NULL
		AND AY20.LX_ATY LIKE ('%SSN VERIFIED%')
		AND (AY20.LX_ATY LIKE ('%ENROLL STA - 11 - FT%') OR AY20.LX_ATY LIKE ('%ENROLL STA - 10 - HT%'))
		AND AY20.LX_ATY LIKE ('%NEW SEP DT%')
		AND COALESCE(TRY_CONVERT(DATE,SUBSTRING(AY20.LX_ATY,CHARINDEX('NEW SEP DT', AY20.LX_ATY) + 13, 10)),'1900-01-01') > CAST(LN16.LD_DLQ_OCC AS DATE) --substitute invalid dates with one that fails the condition to avoid errors
		AND CONCAT(SUBSTRING(AY20.LX_ATY,CHARINDEX('SSN VERIFIED', AY20.LX_ATY) + 15, 3), SUBSTRING(AY20.LX_ATY,CHARINDEX('SSN VERIFIED', AY20.LX_ATY) + 19, 2), SUBSTRING(AY20.LX_ATY,CHARINDEX('SSN VERIFIED', AY20.LX_ATY) + 22, 4)) = LN10.LF_STU_SSN
) POP;

BEGIN TRY
BEGIN TRANSACTION

	INSERT INTO CLS.dbo.ArcAddProcessing
			   (ArcTypeId
			   ,AccountNumber
			   ,ARC
			   ,ScriptId
			   ,ProcessOn
			   ,Comment
			   ,IsReference
			   ,IsEndorser
			   ,CreatedAt
			   ,CreatedBy)
	OUTPUT INSERTED.ArcAddProcessingId, INSERTED.AccountNumber INTO @ArcAddProcessingIds(ArcAddProcessingId, AccountNumber)
	SELECT DISTINCT
		0,
		POP.AccountNumber,
		@ARC,
		@SCRIPTID,
		GETDATE(),
		@COMMENT,
		0,
		0,
		GETDATE(),
		SUSER_NAME()
	FROM
		(
			SELECT DISTINCT	
				AccountNumber
			FROM
				@Pop
		) POP
		LEFT JOIN CLS..ArcAddProcessing ExistingAAP
			ON ExistingAAP.AccountNumber = POP.AccountNumber
			AND ExistingAAP.ARC = @Arc
			AND ExistingAAP.ScriptId = @ScriptId
			AND ExistingAAP.Comment = @Comment
			AND CAST(ExistingAAP.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
	WHERE
		ExistingAAP.AccountNumber IS NULL;

	INSERT INTO CLS.dbo.ArcLoanSequenceSelection
			   (ArcAddProcessingId
			   ,LoanSequence)
	SELECT
		ARC.ArcAddProcessingId,
		POP.LoanSequence
	FROM
		@Pop POP
		INNER JOIN @ArcAddProcessingIds ARC
			ON POP.AccountNumber = ARC.AccountNumber

		COMMIT TRANSACTION;

END TRY
	--write message to process logger if an error occurs
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @SCRIPTID + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;