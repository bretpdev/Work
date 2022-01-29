BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @ARC CHAR(5) = 'CODFP',
				@COMMENT CHAR(1) = ' ',
				@ScriptId VARCHAR(10) = 'UTNWS95';

		INSERT INTO CLS..ArcAddProcessing
		(
			ArcTypeId, 
			ArcResponseCodeId, 
			AccountNumber, 
			RecipientId, 
			ARC, 
			ScriptId, 
			ProcessOn, 
			Comment, 
			IsReference, 
			IsEndorser, 
			CreatedAt, 
			CreatedBy
		)
		SELECT DISTINCT
			1 AS ArcTypeId, --All loans
			NULL AS ArcResponseCodeId,
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			@ARC AS ARC,
			@ScriptId AS ScriptId,
			GETDATE() AS ProcessOn,
			@COMMENT AS Comment,
			0 AS IsReference,
			0 AS IsEndorser,
			GETDATE() AS CreatedAt,
			SUSER_SNAME() AS CreatedBy
		FROM
			CDW..PD10_PRS_NME PD10
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.LA_CUR_PRI > 0.00
			INNER JOIN CDW..DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND DW01.LN_SEQ = LN10.LN_SEQ
				AND DW01.WC_DW_LON_STA = '03'
			INNER JOIN CDW..LN16_LON_DLQ_HST LN16
				ON LN16.BF_SSN = LN10.BF_SSN
				AND LN16.LN_SEQ = LN10.LN_SEQ
				AND LN16.LC_STA_LON16 = '1' --active
				AND LN16.LN_DLQ_MAX BETWEEN 1 AND 5
			INNER JOIN 
			(/*gets only those whose minimum bill date is within last 7 days*/
				SELECT
					MinBill.BF_SSN,
					MinBill.LN_SEQ
				FROM 
					(/*gets minimum bill date for all loans*/
						SELECT
							BF_SSN,
							LN_SEQ,
							MIN(LD_BIL_DU_LON) OVER(PARTITION BY BF_SSN, LN_SEQ) AS LD_BIL_DU_LON
						FROM 
							CDW..LN80_LON_BIL_CRF
						WHERE 
							LC_STA_LON80 = 'A' --active
							AND LC_BIL_TYP_LON = 'P'
					) MinBill
				WHERE 
					CAST(MinBill.LD_BIL_DU_LON AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE)
			) LN80
				ON LN80.BF_SSN = LN10.BF_SSN
				AND LN80.LN_SEQ = LN10.LN_SEQ
			INNER JOIN CDW..LN90_FIN_ATY LN90
				ON LN90.BF_SSN = LN10.BF_SSN
				AND LN90.LN_SEQ = LN10.LN_SEQ
				AND LN90.LC_STA_LON90 = 'A' --active
				AND ISNULL(LN90.LC_FAT_REV_REA,'') = '' --active (transaction not reversed)
				AND LN90.PC_FAT_TYP = '01'
				AND LN90.PC_FAT_SUB_TYP = '01'
			INNER JOIN CDW..PD40_PRS_PHN PD40
				ON PD40.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD40.DI_PHN_VLD = 'Y'
				AND PD40.DC_PHN IN ('A','H','W')
				AND PD40.DC_ALW_ADL_PHN IN ('L','P','X')
			LEFT JOIN 
			(/*Find ARCs left on account in last 20 days ON LOAN LEVEL*/
				SELECT 
					LN85.BF_SSN,
					LN85.LN_SEQ
				FROM
					CDW..AY10_BR_LON_ATY AY10
					INNER JOIN CDW..LN85_LON_ATY LN85
						ON LN85.BF_SSN = AY10.BF_SSN
						AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE 
					AY10.PF_REQ_ACT = 'CODFP'
					AND AY10.LC_STA_ACTY10 = 'A'
					AND CAST(AY10.LD_ATY_REQ_RCV AS DATE) BETWEEN CAST(DATEADD(DAY,-20,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) 
			) ARC
				ON ARC.BF_SSN = LN10.BF_SSN
				AND ARC.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN CLS..ArcAddProcessing AAP --prevents duplicate same-day entries
				ON AAP.AccountNumber = PD10.DF_SPE_ACC_ID
				AND AAP.ARC = @ARC
				AND AAP.ScriptId = @ScriptId
				AND CAST(AAP.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
		WHERE
			ARC.BF_SSN IS NULL
			AND AAP.AccountNumber IS NULL;

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;