DECLARE @RUNDATETIME DATETIME = GETDATE();
DECLARE @WEEKAGO DATE = DATEADD(DAY,-7,@RUNDATETIME),
		@YESTERDAY DATE = DATEADD(DAY,-1,@RUNDATETIME),
		@SCRIPT_ID VARCHAR(10)= 'UTLWS67',
		@ARC VARCHAR(5) = 'G303M';

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
		SELECT DISTINCT
			1 AS ArcTypeId,
			NULL AS	ArcResponseCodeId,
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			'' AS RecipientId,
			@ARC AS ARC,
			NULL AS ActivityType,
			NULL AS ActivityContact,
			@SCRIPT_ID AS ScriptId,
			@RUNDATETIME AS ProcessOn, 
			'Forbearance processed by ' + ISNULL(MaxAppliedForb.LF_USR_CRT_REQ_FOR,'') + ' on ' + ISNULL(RTRIM(CONVERT(VARCHAR(10),MaxAppliedForb.LD_FOR_APL,101)),'') AS Comment, --change here and in left join to ArcAddProcessing at the end of the query
			0 AS IsReference,
			0 AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			'' AS RegardsTo,
			'' AS RegardsCode,
			NULL AS LN_ATY_SEQ,
			0 AS ProcessingAttempts, --this gets updated by the arc add script so it is initialized as 0 attempts made
			@RUNDATETIME AS CreatedAt,
			@SCRIPT_ID AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.LA_CUR_PRI > 0.00
			INNER JOIN 
			(
				SELECT DISTINCT
					LN60.BF_SSN,
					LN60.LN_SEQ,
					LN60.LD_FOR_APL,
					FB10.LF_USR_CRT_REQ_FOR
				FROM
					UDW..LN60_BR_FOR_APV LN60
					INNER JOIN UDW..FB10_BR_FOR_REQ FB10
						ON FB10.BF_SSN = LN60.BF_SSN
						AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
						AND FB10.LC_STA_FOR10 = 'A'
						AND FB10.LC_FOR_STA = 'A'
						AND FB10.LC_FOR_TYP = '05' --Temp hardship
					INNER JOIN
					(
						SELECT
							LN60.BF_SSN,
							LN60.LN_SEQ,
							MAX(LN60.LD_FOR_APL) AS Max_LD_FOR_APL
						FROM
							UDW..LN60_BR_FOR_APV LN60
							INNER JOIN UDW..FB10_BR_FOR_REQ FB10
								ON FB10.BF_SSN = LN60.BF_SSN
								AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
								AND FB10.LC_STA_FOR10 = 'A'
								AND FB10.LC_FOR_STA = 'A'
								AND FB10.LC_FOR_TYP = '05' --Temp hardship
						WHERE
							LN60.LC_STA_LON60 = 'A'
							AND LN60.LC_FOR_RSP != '003' --Not denied
							AND CAST(LN60.LD_FOR_APL AS DATE) >= @WEEKAGO -- applied within reporting period
							AND CAST(LN60.LD_FOR_APL AS DATE) <= @YESTERDAY -- applied within reporting period
						GROUP BY
							LN60.BF_SSN,
							LN60.LN_SEQ
					) MaxDate
						ON MaxDate.BF_SSN = LN60.BF_SSN
						AND MaxDate.LN_SEQ = LN60.LN_SEQ
						AND MaxDate.Max_LD_FOR_APL = LN60.LD_FOR_APL
			) MaxAppliedForb
				ON MaxAppliedForb.BF_SSN = LN10.BF_SSN
				AND MaxAppliedForb.LN_SEQ = LN10.LN_SEQ
			INNER JOIN
			(
				SELECT DISTINCT
					LN60.BF_SSN,
					LN60.LN_SEQ,
					LN60.LF_FOR_CTL_NUM,
					FB10.LF_USR_CRT_REQ_FOR,
					LN60.LD_FOR_APL,
					ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, LN60.LD_FOR_END + 1)/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS MONTHS_USED
				FROM
					UDW..LN60_BR_FOR_APV LN60
					INNER JOIN UDW..FB10_BR_FOR_REQ FB10
						ON FB10.BF_SSN = LN60.BF_SSN
						AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
						AND FB10.LC_STA_FOR10 = 'A'
						AND FB10.LC_FOR_STA = 'A'
						AND FB10.LC_FOR_TYP = '05' --Temp hardship
				WHERE
					LN60.LC_STA_LON60 = 'A'
					AND LN60.LC_FOR_RSP != '003' --Not denied
					AND CAST(LN60.LD_FOR_APL AS DATE) >= @WEEKAGO -- applied within reporting period
					AND CAST(LN60.LD_FOR_APL AS DATE) <= @YESTERDAY -- applied within reporting period
			) ForbInLastWeek
				ON ForbInLastWeek.BF_SSN = LN10.BF_SSN
				AND ForbInLastWeek.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN
			(
				SELECT DISTINCT
					LN60.BF_SSN,
					LN60.LN_SEQ,
					ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, LN60.LD_FOR_END + 1)/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS MONTHS_USED
				FROM
					UDW..LN60_BR_FOR_APV LN60
					INNER JOIN UDW..FB10_BR_FOR_REQ FB10
						ON FB10.BF_SSN = LN60.BF_SSN
						AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
						AND FB10.LC_FOR_TYP = '05'
						AND FB10.LC_FOR_STA = 'A'
						AND FB10.LC_STA_FOR10 = 'A'		
				WHERE
					LN60.LC_STA_LON60 = 'A'
					AND LN60.LC_FOR_RSP != '003' --Not denied
			) PrevTimeUsed
				ON PrevTimeUsed.BF_SSN = ForbInLastWeek.BF_SSN
				AND PrevTimeUsed.LN_SEQ = ForbInLastWeek.LN_SEQ
			LEFT JOIN
			(--AY10Ex: Exclude borrowers with a FBEMA ARC within 2 weeks before or after the forbearance processing date
				SELECT DISTINCT
					LN85.BF_SSN,
					LN85.LN_SEQ,
					AY10.LD_ATY_REQ_RCV
				FROM
					UDW..AY10_BR_LON_ATY AY10
					INNER JOIN UDW..LN85_LON_ATY LN85
						ON AY10.BF_SSN = LN85.BF_SSN
						AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
				WHERE
					PF_REQ_ACT = 'FBEMA'
					AND LC_STA_ACTY10 = 'A'
			) AY10Ex 
				ON AY10Ex.BF_SSN = ForbInLastWeek.BF_SSN
				AND AY10Ex.LN_SEQ = ForbInLastWeek.LN_SEQ
				AND AY10Ex.LD_ATY_REQ_RCV >= DATEADD(DAY, -14, CAST(ForbInLastWeek.LD_FOR_APL AS DATE))
				AND AY10Ex.LD_ATY_REQ_RCV <= DATEADD(DAY, 14, CAST(ForbInLastWeek.LD_FOR_APL AS DATE))
			LEFT JOIN 
			(--G303M_36Q: Exclude borrowers with an open G303M arc task or an open 36 queue task
				SELECT
					BF_SSN
				FROM
					UDW..WQ20_TSK_QUE
				WHERE
					WC_STA_WQUE20 NOT IN ('X','C')
					AND	
					(
						PF_REQ_ACT = @ARC 
						OR WF_QUE = '36'
					)
			) G303M_36Q 
				ON G303M_36Q.BF_SSN = ForbInLastWeek.BF_SSN
			LEFT JOIN
			(-- USERMOD: identify the applied date for the min forb seq for user modified forbearances (user modified forbearances have an active and inactive row in Ln60 with the same LF_FOR_CTL_NUM)
				SELECT
					LN60A.BF_SSN,
					LN60A.LN_SEQ,
					LN60M.LD_FOR_APL --applied date of the minimum forbearance sequence
				FROM
					--borrower has active and inactive rows in the Ln60 table with the same LF_FOR_CTL_NUM
					UDW..LN60_BR_FOR_APV LN60A -- active record for the forbearance
					INNER JOIN UDW..LN60_BR_FOR_APV LN60I -- inactive record for the forbearance
						ON LN60A.BF_SSN = LN60I.BF_SSN
						AND LN60A.LN_SEQ = LN60I.LN_SEQ
						AND (
								LN60A.LD_FOR_BEG = LN60I.LD_FOR_BEG
								OR LN60A.LD_FOR_END = LN60I.LD_FOR_END
							)
					--MINSEQ: minimum forbearce sequence number
					INNER JOIN
					(
						SELECT
							BF_SSN,
							LN_SEQ,
							LF_FOR_CTL_NUM,
							MIN(LN_FOR_OCC_SEQ) AS LN_FOR_OCC_SEQ
						FROM 
							UDW..LN60_BR_FOR_APV
						GROUP BY
							BF_SSN,
							LN_SEQ,
							LF_FOR_CTL_NUM
					) MINSEQ
						ON LN60I.BF_SSN = MINSEQ.BF_SSN
						AND LN60I.LN_SEQ = MINSEQ.LN_SEQ
						AND LN60I.LF_FOR_CTL_NUM = MINSEQ.LF_FOR_CTL_NUM
					--get applied date of forbearance with minimum sequence number
					INNER JOIN UDW..LN60_BR_FOR_APV LN60M
						ON MINSEQ.BF_SSN = LN60M.BF_SSN
						AND MINSEQ.LN_SEQ = LN60M.LN_SEQ
						AND MINSEQ.LF_FOR_CTL_NUM = LN60M.LF_FOR_CTL_NUM
						AND MINSEQ.LN_FOR_OCC_SEQ = LN60M.LN_FOR_OCC_SEQ       
				WHERE
					LN60A.LC_STA_LON60 = 'A'
					AND LN60I.LC_STA_LON60 = 'I'
			) USERMOD
				ON ForbInLastWeek.BF_SSN = USERMOD.BF_SSN
				AND ForbInLastWeek.LN_SEQ = USERMOD.LN_SEQ
			--left join to ArcAddProcessing to check for an existing row to prevent duplicates
			LEFT JOIN ULS..ArcAddProcessing AAP
				ON PD10.DF_SPE_ACC_ID = AAP.AccountNumber
				AND AAP.ARC = @ARC
				AND AAP.Comment = 'Forbearance processed by ' + ISNULL(ForbInLastWeek.LF_USR_CRT_REQ_FOR,'') + ' on ' + ISNULL(RTRIM(CONVERT(VARCHAR,ForbInLastWeek.LD_FOR_APL,101)),'')
		WHERE
			AY10Ex.BF_SSN IS NULL --Exclude borrowers with a FBEMA ARC within 2 weeks of the forbearance processing date
			AND G303M_36Q.BF_SSN IS NULL --Exclude borrowers with an open G303M arc task or an open 36 queue task
			AND AAP.AccountNumber IS NULL --exclude duplicates
			AND (PrevTimeUsed.MONTHS_USED - ForbInLastWeek.MONTHS_USED) >= 36
			AND (--Exclude borrowers with a user modified forbearance unless the applied date was within the reporting period
					USERMOD.BF_SSN IS NULL  --no user modified forbearance
					OR CAST(USERMOD.LD_FOR_APL AS DATE) BETWEEN @WEEKAGO AND @YESTERDAY  --applied date within the reporting period
				);

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @SCRIPT_ID + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@RUNDATETIME,@RUNDATETIME,@SCRIPT_ID,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;
