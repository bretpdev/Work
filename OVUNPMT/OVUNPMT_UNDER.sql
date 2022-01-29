--UnderPayments
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @CurrentDateTime DATETIME = GETDATE();
DECLARE @LookBackDate DATE = DATEADD(DAY,-7,@CurrentDateTime);
DECLARE @ScriptId VARCHAR(20) = 'OVUNPMT';
DECLARE @ARC VARCHAR(5) = 'UNDPT';

BEGIN TRY
	BEGIN TRANSACTION

		INSERT INTO ULS..ArcAddProcessing
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
				NULL AS ArcResponseCodeId,
				NewData.AccountNumber AS AccountNumber,
				'' AS RecipientId,
				@ARC AS ARC,
				NULL AS ActivityType,
				NULL AS ActivityContact,
				@ScriptId AS ScriptId,
				@CurrentDateTime AS ProcessOn,
				'Borrower from designated states made an underpayment on ' + COALESCE(RTRIM(CONVERT(VARCHAR,NewData.PostDate,101)),'') + ' and is delinquent' AS Comment,
				0 AS IsReference,
				0 AS IsEndorser,
				NULL AS ProcessFrom,
				NULL AS ProcessTo,
				NULL AS NeededBy,
				NULL AS RegardsTo,
				NULL AS RegardsCode,
				NULL AS LN_ATY_SEQ,
				0 AS ProcessingAttempts,
				@CurrentDateTime AS CreatedAt,
				@ScriptId AS CreatedBy,
				NULL AS ProcessedAt
			FROM
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID AS AccountNumber,
						MAX_PMT.MAX_LD_FAT_PST AS PostDate,
						ABS(SUM(ISNULL(LN90.LA_FAT_NSI,0.00) + ISNULL(LN90.LA_FAT_CUR_PRI,0.00) + ISNULL(LN90.LA_FAT_LTE_FEE,0.00))) AS AMOUNT,
						SUM(ISNULL(LN80.LA_BIL_CUR_DU,0) + ISNULL(LN80.LA_BIL_PAS_DU,0)) AS AMT_DUE
					FROM 
						UDW..PD10_PRS_NME PD10
						INNER JOIN UDW..PD30_PRS_ADR PD30
							ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
							AND PD30.DC_ADR = 'L'
							AND PD30.DI_VLD_ADR = 'Y'
						INNER JOIN ULS.ovunpmt.States States
							ON States.Abbreviation = PD30.DC_DOM_ST
							AND States.Active = 1
						INNER JOIN UDW..LN10_LON LN10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
							AND LN10.LA_CUR_PRI > 0.00
							AND LN10.LC_STA_LON10 = 'R'
						INNER JOIN UDW..DW01_DW_CLC_CLU DW01
							ON DW01.BF_SSN = LN10.BF_SSN
							AND DW01.LN_SEQ = LN10.LN_SEQ
							AND DW01.WC_DW_LON_STA = '03' -- in repayment
							AND ISNULL(DW01.WX_OVR_DW_LON_STA, '') = ''
						INNER JOIN UDW..LN16_LON_DLQ_HST LN16
							ON LN16.BF_SSN = LN10.BF_SSN
							AND LN16.LN_SEQ = LN10.LN_SEQ
							AND LN16.LC_STA_LON16 = '1' -- active delinquency
							AND LN16.LN_DLQ_MAX >= 4
						INNER JOIN UDW..LN90_FIN_ATY LN90
							ON LN90.BF_SSN = LN10.BF_SSN
							AND LN90.LN_SEQ = LN10.LN_SEQ
							AND LN90.LC_STA_LON90 = 'A'
							AND ISNULL(RTRIM(LN90.LC_FAT_REV_REA),'') = ''
							AND LN90.PC_FAT_TYP = '10'
							AND LN90.PC_FAT_SUB_TYP IN ('10','35')	
						INNER JOIN 
						(
							SELECT
								LN80.BF_SSN,
								LN80.LN_SEQ,
								MAX(LN80.LD_BIL_CRT) AS LD_BIL_CRT,
								MAX(LN80.LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
								MAX(LN80.LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
							FROM
								UDW..BL10_BR_BIL BL10
								INNER JOIN UDW..LN80_LON_BIL_CRF LN80
									ON LN80.BF_SSN = BL10.BF_SSN
									AND LN80.LD_BIL_CRT = BL10.LD_BIL_CRT
									AND LN80.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
							WHERE
								LN80.LC_STA_LON80 = 'A'
								AND LN80.LC_BIL_TYP_LON = 'P'
								AND BL10.LC_IND_BIL_SNT IN ( '1','2','4','7','G','A','B','C','D','E','F','H','I','J','K','L','M','P','Q','R','8','T') -- bill sent
							GROUP BY
								LN80.BF_SSN,
								LN80.LN_SEQ
						) CUR_BILL
							ON CUR_BILL.BF_SSN = LN10.BF_SSN
							AND CUR_BILL.LN_SEQ = LN10.LN_SEQ
						INNER JOIN UDW..LN80_LON_BIL_CRF LN80 --used to get AMT_DUE
							ON LN80.BF_SSN = CUR_BILL.BF_SSN
							AND LN80.LN_SEQ = CUR_BILL.LN_SEQ
							AND LN80.LD_BIL_CRT = CUR_BILL.LD_BIL_CRT
							AND LN80.LN_SEQ_BIL_WI_DTE = CUR_BILL.LN_SEQ_BIL_WI_DTE
							AND LN80.LN_BIL_OCC_SEQ = CUR_BILL.LN_BIL_OCC_SEQ
							AND LN80.LC_STA_LON80 = 'A'
							AND LN80.LC_BIL_TYP_LON = 'P'
						INNER JOIN
						(
							SELECT
								LN90.BF_SSN,
								LN90.LN_SEQ,
								MAX(LN90.LD_FAT_PST) AS MAX_LD_FAT_PST
							FROM 
								UDW..LN10_LON LN10
								INNER JOIN UDW..LN90_FIN_ATY LN90
									ON LN90.BF_SSN = LN10.BF_SSN
									AND LN90.LN_SEQ = LN10.LN_SEQ
							WHERE
								LN10.LA_CUR_PRI > 0.00 
								AND LN10.LC_STA_LON10 = 'R'
								AND LN90.LC_STA_LON90 = 'A'
								AND ISNULL(RTRIM(LN90.LC_FAT_REV_REA),'') = ''
								AND LN90.PC_FAT_TYP = '10'
								AND LN90.PC_FAT_SUB_TYP IN ('10','35')
								AND LN90.LD_FAT_PST >= @LookBackDate --payment effective within target date range
							GROUP BY
								LN90.BF_SSN,
								LN90.LN_SEQ
						) MAX_PMT
							ON MAX_PMT.BF_SSN = LN10.BF_SSN
							AND MAX_PMT.LN_SEQ = LN10.LN_SEQ
						INNER JOIN
						(
							SELECT DISTINCT
								LN10.BF_SSN,
								COUNT(LN10.LN_SEQ) AS LoanCount
							FROM
								UDW..LN10_LON LN10
							WHERE
								LN10.LA_CUR_PRI > 0.00
								AND LN10.LC_STA_LON10 = 'R'
							GROUP BY
								LN10.BF_SSN
						) CountLoans
							ON CountLoans.BF_SSN = LN10.BF_SSN
						LEFT JOIN
						(
							SELECT
								BF_SSN,
								CAST(MAX(LD_ATY_REQ_RCV) AS DATE) AS LD_ATY_REQ_RCV
							FROM
								UDW..AY10_BR_LON_ATY
							WHERE
								PF_REQ_ACT = @ARC
								AND LC_STA_ACTY10 = 'A'
							GROUP BY
								BF_SSN
						) AY10
							ON LN10.BF_SSN = AY10.BF_SSN
							AND AY10.LD_ATY_REQ_RCV >= MAX_PMT.MAX_LD_FAT_PST
						LEFT JOIN ULS..ArcAddProcessing ExistingData
							ON ExistingData.AccountNumber = PD10.DF_SPE_ACC_ID
							AND ExistingData.ARC = @ARC
							AND 
							(
								(
									CAST(ExistingData.CreatedAt AS DATE) =  CAST(@CurrentDateTime AS DATE) --to remove anyone added today to prevent duplicates in recovery
									AND ExistingData.Comment = 'Borrower from designated states made an underpayment on ' + COALESCE(RTRIM(CONVERT(VARCHAR,MAX_PMT.MAX_LD_FAT_PST,101)),'') + ' and is delinquent'
								)
								OR ExistingData.ProcessedAt IS NULL
							)
					WHERE
						LN90.LD_FAT_PST BETWEEN CAST(CUR_BILL.LD_BIL_CRT AS DATE) AND MAX_PMT.MAX_LD_FAT_PST
						AND AY10.BF_SSN IS NULL --exclude borrowers which already have an active underpayment ARC
						AND ExistingData.AccountNumber IS NULL --exclude borrowers who already have an arcadd record
						AND CountLoans.LoanCount > 1 --Excludes single loan borrowers from populating
					GROUP BY
						PD10.DF_SPE_ACC_ID,
						MAX_PMT.MAX_LD_FAT_PST
				) NewData
			WHERE
				NewData.AMOUNT < NewData.AMT_DUE 

	COMMIT TRANSACTION;
END TRY
	--write message to process logger if an error occurs
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

THROW;
END CATCH;