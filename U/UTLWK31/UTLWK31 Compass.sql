--UTLWK31 Compass R3 : References With Foreign Address 
DECLARE @R3Arc VARCHAR(5) ='KFRGN',
		@ScriptId VARCHAR(10) = 'UTLWK31',
		@NOW DATETIME = GETDATE();
DECLARE @TODAY DATE = @NOW;

BEGIN TRY
	BEGIN TRANSACTION

	--R3 Compass
	INSERT INTO ULS..ArcAddProcessing
	(
		ArcTypeId, 
		AccountNumber, 
		RecipientId,
		ARC, 
		ScriptId, 
		ProcessOn, 
		Comment, 
		IsReference, 
		IsEndorser,
		ProcessFrom,
		ProcessTo,
		RegardsTo,
		RegardsCode,
		ProcessingAttempts, 
		CreatedAt, 
		CreatedBy
	)
	SELECT DISTINCT
		1 AS ArcTypeId,--adds arc to all loans
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		NULL AS RecipientId,
		@R3Arc AS ARC,
		@ScriptId AS ScriptId,
		@NOW AS ProcessOn,
		CONCAT(CAST(RF10.BF_RFR AS VARCHAR(10)), ' - ''Review Foreign Address Demographics for Hygiene''') AS Comment,
		0 AS IsReference,
		0 AS IsEndorser,
		NULL AS ProcessFrom,
		NULL AS ProcessTo,
		NULL AS RegardsTo,
		NULL AS RegardsCode,
		0 AS ProcessingAttempts,
		@NOW AS CreatedAt,
		SUSER_SNAME() AS CreatedBy
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..RF10_RFR RF10
			ON RF10.BF_SSN = LN10.BF_SSN
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN UDW..PD30_PRS_ADR PD30 --reference address
			ON PD30.DF_PRS_ID = RF10.BF_RFR
		LEFT JOIN
		(--exclude most recent active MFRGN arc using AY20 extracted reference number
			SELECT
				SUBSTRING(AY20.LX_ATY,1,9) AS ReferenceNumber,
				MAX(AY10.LD_ATY_REQ_RCV) AS max_LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY AY10
				INNER JOIN UDW..AY15_ATY_CMT AY15
					ON AY10.BF_SSN = AY15.BF_SSN
					AND AY10.LN_ATY_SEQ = AY15.LN_ATY_SEQ
				INNER JOIN UDW..AY20_ATY_TXT AY20
					ON AY15.BF_SSN = AY20.BF_SSN
					AND AY15.LN_ATY_SEQ = AY20.LN_ATY_SEQ
					AND AY15.LN_ATY_CMT_SEQ = AY20.LN_ATY_CMT_SEQ
			WHERE
				AY10.PF_REQ_ACT = 'MFRGN'
				AND AY10.LC_STA_ACTY10 = 'A' --active
				AND AY15.LC_STA_AY15 = 'A' --active
				AND PATINDEX('% - ''Review Foreign Address Demographics for Hygiene''%', AY20.LX_ATY) > 0 --returns zero if pattern not found, so any match will be >0
			GROUP BY
				SUBSTRING(AY20.LX_ATY,1,9)
		) AY10_EXTRACT
			ON AY10_EXTRACT.ReferenceNumber = RF10.BF_RFR
			AND PD30.DD_VER_ADR < ISNULL(AY10_EXTRACT.max_LD_ATY_REQ_RCV,CONVERT(DATE,'19000101')) --gets references where address updated before arc (to exclude in WHERE)
		LEFT JOIN 
		(--exclude active KF01 queue using AY20 extracted reference number
			SELECT
				SUBSTRING(AY20.LX_ATY,1,9) AS ReferenceNumber
			FROM
				UDW..WQ20_TSK_QUE WQ20
				INNER JOIN UDW..AY10_BR_LON_ATY AY10
					ON WQ20.BF_SSN = AY10.BF_SSN
					AND WQ20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				INNER JOIN UDW..AY15_ATY_CMT AY15
					ON AY10.BF_SSN = AY15.BF_SSN
					AND AY10.LN_ATY_SEQ = AY15.LN_ATY_SEQ
				INNER JOIN UDW..AY20_ATY_TXT AY20
					ON AY15.BF_SSN = AY20.BF_SSN
					AND AY15.LN_ATY_SEQ = AY20.LN_ATY_SEQ
					AND AY15.LN_ATY_CMT_SEQ = AY20.LN_ATY_CMT_SEQ
			WHERE
				AY10.LC_STA_ACTY10 = 'A' --active
				AND AY15.LC_STA_AY15 = 'A' --active
				AND WQ20.WF_QUE = 'KF'
				AND WQ20.WF_SUB_QUE = '01'
				AND WQ20.WC_STA_WQUE20 IN ('A','H','I','P','U','W') --everything except X & C
				AND PATINDEX('% - ''Review Foreign Address Demographics for Hygiene''%', AY20.LX_ATY) > 0 --returns zero if pattern not found, so any match will be >0
		) AY20_EXTRACT
			ON AY20_EXTRACT.ReferenceNumber = RF10.BF_RFR
		LEFT JOIN ULS..ArcAddProcessing ExistingAAP
			ON ExistingAAP.AccountNumber = PD10.DF_SPE_ACC_ID
			AND ExistingAAP.ARC = @R3Arc
			AND ExistingAAP.ScriptId = @ScriptId
			AND ISNULL(ExistingAAP.Comment,'') = CONCAT(CAST(RF10.BF_RFR AS VARCHAR(10)), ' - ''Review Foreign Address Demographics for Hygiene''')
			AND (
					CAST(ExistingAAP.CreatedAt AS DATE) = @TODAY --prevents multiple arcs from being added on the same day
					OR ExistingAAP.ProcessedAt IS NULL
				)
	WHERE
		AY10_EXTRACT.ReferenceNumber IS NULL --exclude most recent active MFRGN arc using AY20 extracted reference number
		AND AY20_EXTRACT.ReferenceNumber IS NULL --exclude active KF01 queue using AY20 extracted reference number
		AND ExistingAAP.AccountNumber IS NULL --no matching existing record
		AND LN10.LA_CUR_PRI > 0.00
		AND LN10.LC_STA_LON10 = 'R'
		AND RF10.BC_STA_REFR10 = 'A'
		AND PD30.DC_ADR = 'L'
		AND PD30.DI_VLD_ADR = 'Y'
		AND PD30.DC_DOM_ST = ''
	;
	COMMIT TRANSACTION;
END TRY
	--write message to process logger if an error occurs
	BEGIN CATCH
		DECLARE @EM_ VARCHAR(4000) = CONCAT(@ScriptId, ' encountered an error.  Transaction not committed. Error: ', (SELECT ERROR_MESSAGE()));

		ROLLBACK TRANSACTION;

		DECLARE @ProcessLogId_ INT,
				@ProcessNotificationId_ INT,
				@NotificationTypeId_ INT = 
					(--Error report
						SELECT TOP 1 
							NotificationTypeId 
						FROM
							ProcessLogs..NotificationTypes 
						WHERE
							NotificationTypeDescription = 'Error Report'
					), 
				@NotificationSeverityTypeId_ INT = 
					(--Critical
						SELECT TOP 1
							NotificationSeverityTypeId 
						FROM
							ProcessLogs..NotificationSeverityTypes 
						WHERE
							NotificationSeverityTypeDescription = 'Critical'
					); 
		
		INSERT INTO ProcessLogs..ProcessLogs 
		(
			StartedOn, 
			EndedOn, 
			ScriptId, 
			Region, 
			RunBy
		) 
		VALUES
		(
			@NOW,
			@NOW,
			@ScriptId,
			'uheaa',
			SUSER_SNAME()
		);
		
		SET @ProcessLogId_ = SCOPE_IDENTITY()

		INSERT INTO ProcessLogs..ProcessNotifications 
		(
			NotificationTypeId,
			NotificationSeverityTypeId,
			ProcessLogId, 
			ResolvedAt, 
			ResolvedBy
		) 
		VALUES
		(
			@NotificationTypeId_,
			@NotificationSeverityTypeId_,
			@ProcessLogId_, 
			NULL, 
			NULL
		);

		SET @ProcessNotificationId_ = SCOPE_IDENTITY()

		INSERT INTO ULS.[log].ProcessLogMessages 
		(
			ProcessNotificationId, 
			LogMessage
		) 
		VALUES
		(
			@ProcessNotificationId_,
			@EM_
		);

	THROW;
END CATCH;
