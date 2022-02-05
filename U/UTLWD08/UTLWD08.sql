DECLARE @ScriptId VARCHAR(20) = 'UTLWD08';
DECLARE @CurrentDateTime DATETIME = GETDATE();
DECLARE @ARC VARCHAR(5) = 'K2REF';

BEGIN TRY
	BEGIN TRANSACTION

	INSERT INTO 
		ULS..ArcAddProcessing
			(
				ArcTypeId,
				ArcResponseCodeId,
				AccountNumber,
				RecipientId,
				ARC,
				ActivityType,
				ActivityContact,
				ScriptId,
				ProcessOn,
				Comment,
				IsReference,
				IsEndorser,
				ProcessFrom,
				ProcessTo,
				NeededBy,
				RegardsTo,
				RegardsCode,
				LN_ATY_SEQ,
				ProcessingAttempts,
				CreatedAt,
				CreatedBy,
				ProcessedAt
			)
	SELECT
		1 AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		NewData.AccountNumber,
		'' AS RecipientId,
		@ARC AS ARC,
		NULL AS ActivityType,
		NULL AS ActivityContact,
		@ScriptId AS ScriptId,
		@CurrentDateTime AS ProcessOn,
		'Account in a Skip Status Without 2 or more References' AS Comment,
		0 AS IsReference,
		0 AS IsEndorser,
		NULL AS ProcessFrom,
		NULL AS ProcessTo,
		NULL AS NeededBy,
		NULL AS RegardsTo,
		NULL AS RegardsCode,
		NULL AS LN_ATY_SEQ,
		0 AS ProcessingAttempts, --this gets updated by the arc add script so it is initialized as 0 attempts made
		@CurrentDateTime AS CreatedAt,
		@ScriptId AS CreatedBy,
		NULL AS ProcessedAt
	FROM
		(
			SELECT DISTINCT 
				PD10.DF_SPE_ACC_ID AS AccountNumber
			FROM
				UDW..PD10_PRS_NME PD10
				INNER JOIN UDW..LN10_LON LN10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN
			--MAX_SKIP: get date of most recent borrower skip
				(
					SELECT
						BF_SSN,
						MAX(DD_STA_SKP) AS DD_STA_SKP
					FROM
						UDW..PD27_PRS_SKP_PRC
					WHERE
						DC_SKP_PRS = '1' -- person is a borrower
						AND DC_STA_SKP = '2' -- in skip
						AND DD_SKP_END  is NULL
					GROUP BY
						BF_SSN
				) MAX_SKIP
					ON MAX_SKIP.BF_SSN = DF_PRS_ID
				INNER JOIN UDW..PD27_PRS_SKP_PRC PD27
					ON PD27.BF_SSN = MAX_SKIP.BF_SSN
					AND PD27.DD_STA_SKP = MAX_SKIP.DD_STA_SKP
			-- RF10: count of active references
				LEFT JOIN
				(
					SELECT
						BF_SSN,
						COUNT(*) AS CNT
					FROM 
						UDW..RF10_RFR
					WHERE
						BC_RFR_REL_BR NOT IN ('05','13','15') --NOT EMPLOYER, PHYSICIAN, LAWYER
						AND
							(
								BC_STA_REFR10 ='A' --ACTIVE REFERENCE
								OR
									(
										BC_STA_REFR10 = 'H' --VALID REFERENCE W/ NO CONTACT REQUEST
										AND BC_REA_RFR_HST IN ('R','D')
									)
							)
					GROUP BY
						BF_SSN
				) RF10
					ON PD27.BF_SSN = RF10.BF_SSN
			--WQ20: open BR01 queue tasks	
				LEFT JOIN
				(
					SELECT DISTINCT
						BF_SSN,
						WF_LST_DTS_WQ20
					FROM
						UDW..WQ20_TSK_QUE
					WHERE
						WF_QUE = 'BR'
						AND WF_SUB_QUE = '01'
						AND WC_STA_WQUE20 NOT IN ('C','X')
				) WQ20
					ON PD27.BF_SSN = WQ20.BF_SSN
			--WQ20CLSD: closedBR01 queue tasks	
				LEFT JOIN
				(
					SELECT DISTINCT
						BF_SSN,
						MAX(WF_LST_DTS_WQ20) AS WF_LST_DTS_WQ20
					FROM
						UDW..WQ21_TSK_QUE_HST
					WHERE
						WF_QUE = 'BR'
						AND WF_SUB_QUE = '01'
						AND WC_STA_WQUE20 IN ('C','X')
					GROUP BY
						BF_SSN
				) WQ21
					ON WQ21.BF_SSN = PD27.BF_SSN
					AND CAST(ISNULL(WQ21.WF_LST_DTS_WQ20,'1900-01-01') AS DATE) >= PD27.DD_SKP_BEG
					AND DATEADD(DAY, 90, CAST(ISNULL(WQ21.WF_LST_DTS_WQ20,'1900-01-01') AS DATE)) >= CAST(@CurrentDateTime AS DATE)
			WHERE
				LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 ='R'
				AND PD27.DC_SKP_PRS = '1' -- person is a borrower
				AND PD27.DC_STA_SKP = '2' -- in skip
				AND ISNULL(RF10.CNT,0) < 2 -- less than 2 active references
				AND WQ20.BF_SSN IS NULL -- no open BR01 queue task
				AND WQ21.BF_SSN IS NULL -- I added this Riley
				AND PD27.DD_SKP_END  IS NULL --skip is still active added
		)NewData
		LEFT JOIN ULS..ArcAddProcessing ExistingData
				ON NewData.AccountNumber = ExistingData.AccountNumber
				AND ExistingData.ARC = @ARC
				AND 
					(
						CAST(ExistingData.CreatedAt AS DATE) =  CAST(@CurrentDateTime AS DATE) --to remove anyone added today to prevent duplicates in recovery
						OR ExistingData.ProcessedAt IS NULL
					)
	WHERE
		ExistingData.AccountNumber IS NULL --exclude borrowers who already have an arcadd record --TODO (FOR TESTING): uncomment


		COMMIT TRANSACTION;

END TRY
	--write message to process logger if an error occurs
	BEGIN CATCH
		DECLARE @EM_ VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

		ROLLBACK TRANSACTION;

		DECLARE @ProcessLogId_ INT;
		DECLARE @ProcessNotificationId_ INT;
		DECLARE @NotificationTypeId_ INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
		DECLARE @NotificationSeverityTypeId_ INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
		INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'uheaa',SUSER_SNAME());
		SET @ProcessLogId_ = SCOPE_IDENTITY()

		INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
		SET @ProcessNotificationId_ = SCOPE_IDENTITY()

		INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;
