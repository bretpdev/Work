BEGIN TRY
BEGIN TRANSACTION

INSERT INTO 
	CLS..ArcAddProcessing
		(
			[ArcTypeId],
			[ArcResponseCodeId],
			[AccountNumber],
			[RecipientId],
			[ARC],
			[ScriptId],
			[ProcessOn],
			[Comment],
			[IsReference],
			[IsEndorser],
			[ProcessFrom],
			[ProcessTo],
			[NeededBy],
			[RegardsTo],
			[RegardsCode],
			[CreatedAt],
			[CreatedBy],
			[ProcessedAt]
		)
SELECT
	1 [ArcTypeId],
	NULL [ArcResponseCodeId],
	POP.DF_SPE_ACC_ID [AccountIdentifier],
	NULL [RecipientId],
	'DISBA' [ARC],
	'UTNWS84' [ScriptId],
	GETDATE() [ProcessOn],
	'Disbursement or Disbursement adjustment exists after discharge. Please review account' [Comment],
	0 [IsReference],
	0 [IsEndorser],
	NULL [ProcessFrom],
	NULL [ProcessTo],
	NULL [NeededBy],
	NULL [RegardsTo],
	NULL [RegardsCode],
	GETDATE() [CreatedAt],
	'UTNWS84' [CreatedBy],
	NULL [ProcessedAt]
FROM
	(
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			DSCH.BF_SSN 		
		FROM
			CDW..PD10_PRS_NME PD10
			INNER JOIN
		 --discharge
			(
				SELECT
					LN90.BF_SSN,
					LN90.LN_SEQ,
					MAX(LN90.LD_FAT_PST) AS LD_FAT_PST
				FROM
					CDW..LN90_FIN_ATY LN90
				WHERE
				LN90.PC_FAT_TYP = '50'
				AND LN90.PC_FAT_SUB_TYP = '02'
				AND LN90.LC_FAT_REV_REA IN ('',NULL)
				AND LN90.LC_STA_LON90 = 'A'
				GROUP BY
					LN90.BF_SSN,
					LN90.LN_SEQ
			) DSCH
				ON PD10.DF_PRS_ID = DSCH.BF_SSN
			INNER JOIN
		--new disbursement
			(
				SELECT
					LN90.BF_SSN,
					LN90.LN_SEQ,
					MAX(LN90.LD_FAT_PST) AS LD_FAT_PST
				FROM 
					CDW..LN90_FIN_ATY LN90
				WHERE
					( (LN90.PC_FAT_TYP = '01' AND LN90.PC_FAT_SUB_TYP = '01') OR (LN90.PC_FAT_TYP = '07' AND LN90.PC_FAT_SUB_TYP IN ('85','86')) )
					AND LN90.LC_FAT_REV_REA IN ('',NULL)
					AND LN90.LC_STA_LON90 = 'A'
				GROUP BY
					LN90.BF_SSN,
					LN90.LN_SEQ
			) DISB
				ON DSCH.BF_SSN = DISB.BF_SSN
				AND DSCH.LN_SEQ = DISB.LN_SEQ
			INNER JOIN CDW..LN10_LON LN10
				ON DISB.BF_SSN = LN10.BF_SSN
				AND DISB.LN_SEQ = LN10.LN_SEQ
				AND LN10.LA_CUR_PRI > 0.00 
				AND LN10.LC_STA_LON10 = 'R'
			LEFT JOIN --open P701 queue tasks
				(
					SELECT 
						LN85.BF_SSN,
						LN85.LN_SEQ
					FROM	
						CDW..WQ20_TSK_QUE WQ20
						INNER JOIN CDW..LN85_LON_ATY LN85
							ON WQ20.BF_SSN = LN85.BF_SSN
							AND WQ20.LN_ATY_SEQ = LN85.LN_ATY_SEQ
					WHERE
						WQ20.WF_QUE = 'P7'
						AND WF_SUB_QUE = '01'
						AND WC_STA_WQUE20 NOT IN ('X','C')
				) QUETASK
					ON DISB.BF_SSN = QUETASK.BF_SSN
					AND DISB.LN_SEQ = QUETASK.LN_SEQ
			LEFT JOIN
		--'ADTLF' or 'APPRV' ARC
			(
				SELECT
					AY10.BF_SSN,
					MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM
					CDW..AY10_BR_LON_ATY AY10
				WHERE
					AY10.PF_REQ_ACT IN ('ADTLF', 'APPRV')
					AND LC_STA_ACTY10 = 'A'
				GROUP BY
					AY10.BF_SSN
			) ADTLF
				ON DSCH.BF_SSN = ADTLF.BF_SSN
				AND ADTLF.LD_ATY_REQ_RCV < DISB.LD_FAT_PST --arc received date < new disbursement date
			LEFT JOIN CLS..ArcAddProcessing AAP
		--prevent duplicates
				ON AAP.AccountNumber = PD10.DF_SPE_ACC_ID
				AND CAST(AAP.CreatedAt AS DATE) >= CAST(DISB.LD_FAT_PST AS DATE) -- to ensure each occurence of a disbursement after a discharge only gets picked up once
				AND DATEDIFF(DAY,CAST(AAP.CreatedAt AS DATE),GETDATE()) > 5 -- no DISBA ARCs within the past 5 days to prevent duplicates in recovery
				AND ARC = 'DISBA'
		WHERE
			DISB.LD_FAT_PST > DSCH.LD_FAT_PST --disbursement after the discharge
			AND QUETASK.BF_SSN IS NULL --no incomplete P701 queue tasks
			AND ADTLF.BF_SSN IS NULL --exclude borrowers  where the borrower has a ADTLF or APPRV arc and the  arc received date (LD_ATY_REQ_RCV) < new disbursement date
			AND AAP.ArcAddProcessingId IS NULL --excludes borrowers who have the arc recently (used for recovery just in case we miss a day)
			--AND PD10.DF_SPE_ACC_ID IN ('4457581639','8608180790','3470621350','0050147296','9724779719','6302265013') --TODO:  for testing, remove for production
	) POP

	

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = 'UTNWS84.sql encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),'UTNWS84','UHEAA',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;