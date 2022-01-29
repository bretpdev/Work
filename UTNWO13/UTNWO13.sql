USE CDW;
GO

DECLARE @MAIN TABLE
(
	 DF_SPE_ACC_ID VARCHAR(10)
	,BF_SSN VARCHAR(9)
	,LD_DFR_BEG DATE
	,LD_DFR_END DATE
	,LC_DFR_STA CHAR(1)
	,LC_STA_DFR10 CHAR(1)
	,DFR_BEG_END_DIFF INT
	,DFR_BEG_TODAY_DIFF INT
);

DECLARE @TODAY DATE = GETDATE(),
		@R2ARC VARCHAR(5) = 'MILRV', --ARC to write
		@R3ARC VARCHAR(5) = 'MILRN', --ARC to write
		@ScriptId VARCHAR(10) = 'UTNWO13',
		@NOW DATETIME = GETDATE(),
		@ArcTypeId TINYINT = 1; --Add arc to all loans

BEGIN TRY
	BEGIN TRANSACTION

	--get type 38 borrowers as MAIN pop
	INSERT INTO @MAIN (DF_SPE_ACC_ID, BF_SSN, LD_DFR_BEG, LD_DFR_END, LC_DFR_STA, LC_STA_DFR10, DFR_BEG_END_DIFF, DFR_BEG_TODAY_DIFF)
	SELECT DISTINCT
		 PD10.DF_SPE_ACC_ID
		,LN10.BF_SSN
		,LN50.LD_DFR_BEG
		,LN50.LD_DFR_END
		,DF10.LC_DFR_STA
		,DF10.LC_STA_DFR10
		,DATEDIFF(DAY, LN50.LD_DFR_BEG, LN50.LD_DFR_END) AS DFR_BEG_END_DIFF
		,DATEDIFF(DAY, LN50.LD_DFR_BEG, @TODAY) AS DFR_BEG_TODAY_DIFF
	FROM
		PD10_PRS_NME PD10
		INNER JOIN LN10_LON LN10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN LN50_BR_DFR_APV LN50
			ON LN50.BF_SSN = LN10.BF_SSN
			AND LN50.LN_SEQ = LN10.LN_SEQ
			AND LN50.LC_STA_LON50 = 'A'
			AND LN50.LC_DFR_RSP != '003' --NOT Denied
			AND @TODAY BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
		INNER JOIN DF10_BR_DFR_REQ DF10
			ON DF10.BF_SSN = LN50.BF_SSN
			AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
			AND DF10.LC_DFR_TYP = '38'
			AND DF10.LC_DFR_STA = 'A'
			AND DF10.LC_STA_DFR10 = 'A'
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0.00
	;
	--select * from @MAIN; --test

--R2 - create a ML task (for borrowers who have a Military Deferment on their account for more than 12 months, to review the active duty status on the DOD website.
	INSERT INTO CLS..ArcAddProcessing (ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessingAttempts, CreatedAt, CreatedBy)
	SELECT DISTINCT
		 @ArcTypeId AS ArcTypeId
		,NewData.DF_SPE_ACC_ID AS AccountNumber
		,@R2ARC AS ARC
		,@ScriptId AS ScriptId
		,@NOW AS ProcessOn
		,'Military Deferment eligibility review required' AS Comment
		,0 AS IsReference
		,0 AS IsEndorser
		,0 AS ProcessingAttempts
		,@NOW AS CreatedAt
		,SUSER_SNAME() AS CreatedBy
	FROM
		(--get MILRV arc info
			SELECT
				 MAIN.DF_SPE_ACC_ID
				 ----TEST FIELDS:
				--,MAIN.BF_SSN
				--,MAIN.LD_DFR_BEG
				--,MAIN.DFR_BEG_END_DIFF
				--,MAIN.DFR_BEG_TODAY_DIFF
				--,AY10_MAX.max_LD_ATY_REQ_RCV
			FROM
				@MAIN MAIN
				LEFT JOIN 
				( --Arc should be evaluated at a borrower level
					SELECT
						BF_SSN
						,CAST(MAX(LD_ATY_REQ_RCV) AS DATE) AS max_LD_ATY_REQ_RCV
					FROM
						AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = @R2ARC 
						AND LC_STA_ACTY10 = 'A'
					GROUP BY
						BF_SSN
				) AY10_MAX
					ON MAIN.BF_SSN = AY10_MAX.BF_SSN					
				LEFT JOIN AY10_BR_LON_ATY AY10X
					ON MAIN.BF_SSN = AY10X.BF_SSN
					AND AY10X.LC_STA_ACTY10 = 'A'
					AND AY10X.PF_REQ_ACT = @R2ARC
					AND CAST(AY10X.LD_ATY_REQ_RCV AS DATE) BETWEEN MAIN.LD_DFR_BEG AND MAIN.LD_DFR_END
			WHERE
				AY10X.BF_SSN IS NULL
				AND	MAIN.DFR_BEG_END_DIFF > 365 --Deferments greater than 1 year long
				AND MAIN.DFR_BEG_TODAY_DIFF >= 365 --Deferment started at least 1 year ago
				AND ISNULL(AY10_MAX.max_LD_ATY_REQ_RCV,'1900-01-01') <= CAST(DATEADD(DAY,-365, @TODAY) AS DATE) --NO MILRV in the last year
		) NewData
		LEFT JOIN CLS..ArcAddProcessing ExistingAAP
			ON ExistingAAP.AccountNumber = NewData.DF_SPE_ACC_ID
			AND ExistingAAP.ARC = @R2ARC
			AND ExistingAAP.ScriptId = @ScriptId
			AND CONVERT(DATE,ExistingAAP.CreatedAt) = @TODAY
	WHERE
		ExistingAAP.AccountNumber IS NULL --no matching existing record
	;

--R3 - create a review task (ML) for borrowers in Military Deferment whose IDR plan PFH payment is about to expire. Due to the HEROES Act we need to maintain the same PFH payment for the borrower for up to 3 years to re-apply the IDR payment as applicable.
	INSERT INTO CLS..ArcAddProcessing (ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessingAttempts, CreatedAt, CreatedBy) --uncomment for promotion
	SELECT DISTINCT
		 @ArcTypeId AS ArcTypeId
		,NewData.DF_SPE_ACC_ID AS AccountNumber
		,@R3ARC AS ARC
		,@ScriptId AS ScriptId
		,@NOW AS ProcessOn
		,'Military IDR extension review required' AS Comment
		,0 AS IsReference
		,0 AS IsEndorser
		,0 AS ProcessingAttempts
		,@NOW AS CreatedAt
		,SUSER_SNAME() AS CreatedBy
	FROM
		(
			SELECT
				 MAIN.DF_SPE_ACC_ID
				----TEST FIELDS:
				--,MAIN.BF_SSN
				--,AY10X.LD_ATY_REQ_RCV
				--,RS10.LD_RPS_1_PAY_DU
				--,LN66_MIN.min_LN_RPS_TRM
				--,DATEADD(MONTH, LN66_MIN.min_LN_RPS_TRM - 1, RS10.LD_RPS_1_PAY_DU) AS IDRmonthbefore
				--,DATEADD(MONTH, LN66_MIN.min_LN_RPS_TRM, RS10.LD_RPS_1_PAY_DU) AS IDREnd
			FROM
				@MAIN MAIN 
				INNER JOIN 
				(
					SELECT 
						  LN66.BF_SSN
						 ,LN66.LN_SEQ
						 ,LN66.LN_RPS_SEQ
						 ,LN66.LN_GRD_RPS_SEQ
						 ,MIN(LN_RPS_TRM) AS min_LN_RPS_TRM
					FROM
						LN65_LON_RPS LN65 
						INNER JOIN LN66_LON_RPS_SPF	LN66
							ON LN66.BF_SSN = LN65.BF_SSN
							AND LN66.LN_SEQ = LN65.LN_SEQ
							AND LN66.LN_RPS_SEQ = LN65.LN_RPS_SEQ
					WHERE
						LN66.LN_GRD_RPS_SEQ = 1
						AND LN65.LC_STA_LON65 = 'A'
						AND LN65.LC_TYP_SCH_DIS IN ('C1','C2','C3','IB','CA','I3','I5') --IDR plans
					GROUP BY
						 LN66.BF_SSN
						,LN66.LN_SEQ
						,LN66.LN_RPS_SEQ
						,LN66.LN_GRD_RPS_SEQ
				) LN66_MIN
					ON LN66_MIN.BF_SSN = MAIN.BF_SSN
				INNER JOIN RS10_BR_RPD RS10
					ON LN66_MIN.BF_SSN = RS10.BF_SSN
					AND LN66_MIN.LN_RPS_SEQ = RS10.LN_RPS_SEQ
					AND RS10.LC_STA_RPST10 = 'A'
				LEFT JOIN AY10_BR_LON_ATY AY10X --EXCLUDE: MILRN between IDR start and end dates
					ON MAIN.BF_SSN = AY10X.BF_SSN 
					AND AY10X.PF_REQ_ACT = @R3ARC
					AND AY10X.LC_STA_ACTY10 = 'A'
					AND CAST(AY10X.LD_ATY_REQ_RCV AS DATE) >= RS10.LD_RPS_1_PAY_DU
					AND CAST(AY10X.LD_ATY_REQ_RCV AS DATE) <= CAST(DATEADD(MONTH, LN66_MIN.min_LN_RPS_TRM, RS10.LD_RPS_1_PAY_DU) AS DATE)
			WHERE
				AY10X.BF_SSN IS NULL
				AND CAST(DATEADD(MONTH, LN66_MIN.min_LN_RPS_TRM - 1, RS10.LD_RPS_1_PAY_DU) AS DATE) <= @TODAY --PFH payment is set to expire a month (30 days) from the current date 
				AND CAST(DATEADD(MONTH, LN66_MIN.min_LN_RPS_TRM, RS10.LD_RPS_1_PAY_DU) AS DATE) >= @TODAY --PFH payment is set to expire a month (30 days) from the current date 
		) NewData
		LEFT JOIN CLS..ArcAddProcessing ExistingAAP
			ON ExistingAAP.AccountNumber = NewData.DF_SPE_ACC_ID
			AND ExistingAAP.ARC = @R3ARC
			AND ExistingAAP.ScriptId = @ScriptId
			AND CONVERT(DATE,ExistingAAP.CreatedAt) = @TODAY
	WHERE
		ExistingAAP.AccountNumber IS NULL --No matching existing record
	;

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
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@NOW,@NOW,@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;
