USE CDW;
GO	

DECLARE @BKIDR VARCHAR(5) = 'BKIDR', --ARC to write
		@ScriptId VARCHAR(10) = 'IDRCHPT13',
		@NOW DATETIME = GETDATE();

BEGIN TRY
	BEGIN TRANSACTION

	INSERT INTO CLS..ArcAddProcessing (ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessingAttempts, CreatedAt, CreatedBy) --uncomment for promotion
	SELECT DISTINCT
		0 AS ArcTypeId, --Atd22ByLoan (Add arc by sequence number)
		NewData.DF_SPE_ACC_ID AS AccountNumber,
		@BKIDR AS ARC,
		@ScriptId AS ScriptId,
		@NOW AS ProcessOn,
		'IDR not recertified. Review and apply Bankruptcy' AS Comment,
		0 AS IsReference,
		0 AS IsEndorser,
		0 AS ProcessingAttempts,
		@NOW AS CreatedAt,
		SUSER_SNAME() AS CreatedBy
	FROM
		(
			SELECT
				PD10.DF_SPE_ACC_ID
				--,LN10.BF_SSN --for testing only
				--,LN10.LN_SEQ --for testing only
				--,AY10_BKOBL.BKOBL_max_LD_ATY_REQ_RCV --for testing only
				--,AY10_BKIDR.BKIDR_max_LD_ATY_REQ_RCV --for testing only				
			FROM
				LN10_LON LN10
				INNER JOIN PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN
				(--get most recent active BKOBL date
					SELECT DISTINCT
						BF_SSN,
						MAX(LD_ATY_REQ_RCV) AS BKOBL_max_LD_ATY_REQ_RCV
					FROM
						AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'BKOBL'
						AND LC_STA_ACTY10 = 'A'
					GROUP BY
						BF_SSN
				) AY10_BKOBL
					ON LN10.BF_SSN = AY10_BKOBL.BF_SSN
				LEFT JOIN 
				(--EXCLUDE: most recent active BKIDR after BKOBL
					SELECT
						BF_SSN,
						MAX(LD_ATY_REQ_RCV) AS BKIDR_max_LD_ATY_REQ_RCV
					FROM
						AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = @BKIDR
						AND LC_STA_ACTY10 = 'A'
					GROUP BY
						BF_SSN
				) AY10_BKIDR
					ON LN10.BF_SSN = AY10_BKIDR.BF_SSN
					AND AY10_BKIDR.BKIDR_max_LD_ATY_REQ_RCV > AY10_BKOBL.BKOBL_max_LD_ATY_REQ_RCV --BKIDR after BKOBL
				LEFT JOIN
				(--EXCLUDE: borrowers in bankruptcy status
					SELECT
						 BF_SSN
						,LN_SEQ
					FROM
						DW01_DW_CLC_CLU
					WHERE
						WC_DW_LON_STA IN ('20', '21')
				) BANKRUPTCY
					ON LN10.BF_SSN = BANKRUPTCY.BF_SSN
					AND LN10.LN_SEQ = BANKRUPTCY.LN_SEQ
				LEFT JOIN
				(--EXCLUDE: open PP queue task
					SELECT
						BF_SSN
					FROM
						WQ20_TSK_QUE
					WHERE
						WF_QUE = 'PP'
						AND WF_SUB_QUE = '01'
						AND WC_STA_WQUE20 NOT IN ('X','C')
				) PPQ
					ON LN10.BF_SSN = PPQ.BF_SSN
				LEFT JOIN
				(--EXCLUDE: currently on IBR
					SELECT
						BF_SSN
					FROM
						LN65_LON_RPS 
					WHERE
						LC_TYP_SCH_DIS  IN ('IB','C1','C2','C3','CA','IA','I3','I5') 
						AND LC_STA_LON65 = 'A'
				) IBR
					ON LN10.BF_SSN = IBR.BF_SSN
			WHERE
				BANKRUPTCY.BF_SSN IS NULL
				AND PPQ.BF_SSN IS NULL
				AND IBR.BF_SSN IS NULL
				AND AY10_BKIDR.BF_SSN IS NULL
				AND LN10.LA_CUR_PRI > 0.00 --open
				AND LN10.LC_STA_LON10 = 'R' --released
		) NewData
		LEFT JOIN CLS..ArcAddProcessing ExistingAAP
			ON ExistingAAP.AccountNumber = NewData.DF_SPE_ACC_ID
			AND ExistingAAP.ARC = @BKIDR
			AND ExistingAAP.ScriptId = @ScriptId
			AND CONVERT(DATE,ExistingAAP.CreatedAt) = CONVERT(DATE,@NOW)
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
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@NOW,@NOW,@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;


--select * from cls..ArcAddProcessing where ScriptId = 'IDRCHPT13';--for testing only