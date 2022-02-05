--OVUNPMTFD_UNDER.sql: UNDERPAYMENTS
DECLARE @CurrentDateTime DATETIME = GETDATE();
DECLARE @LookBackDate DATE = DATEADD(DAY,-7,@CurrentDateTime),
		@ScriptId VARCHAR(20) = 'OVUNPMTFD',
		@ARC VARCHAR(5) = 'UNDPT';--underpayments

BEGIN TRY
	BEGIN TRANSACTION

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
		ProcessFrom,
		ProcessTo,
		NeededBy,
		RegardsTo,
		RegardsCode,
		CreatedAt,
		CreatedBy,
		ProcessedAt
	)
	SELECT
		1 AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		InsertData.AccountNumber,
		NULL AS RecipientId,
		@ARC AS ARC,
		@ScriptId AS ScriptId,
		@CurrentDateTime AS ProcessOn,
		'Borrower from designated states made an underpayment on ' + COALESCE(RTRIM(CONVERT(VARCHAR(10),InsertData.PmtPostedDate,101)),'') + ' and is delinquent' AS Comment,
		0 AS IsReference,
		0 AS IsEndorser,
		NULL AS ProcessFrom,
		NULL AS ProcessTo,
		NULL AS NeededBy,
		NULL AS RegardsTo,
		NULL AS RegardsCode,
		@CurrentDateTime AS CreatedAt,
		@ScriptId AS CreatedBy,
		NULL AS ProcessedAt
	FROM 
		(
			SELECT
				NewData.DF_SPE_ACC_ID AS AccountNumber,
				NewData.LD_FAT_PST AS PmtPostedDate
			FROM
			--NewData
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID,
						MAX_PMT.MAX_LD_FAT_PST AS LD_FAT_PST
					FROM
						CDW..PD10_PRS_NME PD10
					--borrower has a valid legal address
						INNER JOIN CDW..PD30_PRS_ADR PD30
							ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
							AND PD30.DC_ADR = 'L'
							AND PD30.DI_VLD_ADR = 'Y'
					--borrower is from a targeted state
						INNER JOIN CLS.ovunpmtfd.States
							ON States.Abbreviation = PD30.DC_DOM_ST
							AND States.Active = 1
					--LN10 borrower has open, released loans
						INNER JOIN CDW..LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
							AND LN10.LA_CUR_PRI > 0.00
							AND LN10.LC_STA_LON10 = 'R'
					--MAX_PMT - MAX payment posted date and payment type (for ARC comment) within lookback period for active, non-reversed 1010 and 1035 payments
						INNER JOIN
						(
							SELECT
								LN10.BF_SSN,
								LN10.LN_SEQ,
								MAX(LD_FAT_PST) AS MAX_LD_FAT_PST
							FROM 
								CDW..LN10_LON LN10
								INNER JOIN CDW..LN90_FIN_ATY LN90
									ON LN10.BF_SSN = LN90.BF_SSN
									AND LN10.LN_SEQ = LN90.LN_SEQ
							WHERE
								LN10.LA_CUR_PRI > 0.00 --open loans
								AND LN10.LC_STA_LON10 = 'R' --released loans
								AND LN90.LC_STA_LON90 = 'A' --active transactions
								AND ISNULL(LN90.LC_FAT_REV_REA,'') = '' --non-reversed transactions
								AND LN90.PC_FAT_TYP = '10' --1010 AND 1035 transactions
								AND LN90.PC_FAT_SUB_TYP IN ('10','35')
								AND LN90.LD_FAT_PST >= @LookBackDate --payment effective within target date range
							GROUP BY
								LN10.BF_SSN,
								LN10.LN_SEQ
						) MAX_PMT
							ON PD10.DF_PRS_ID = MAX_PMT.BF_SSN
					--PMT calculate borrower payments the same way UTNWF07 calculates them so they can be compared to the amount in the PA queue task which comes from UTNWF07
						INNER JOIN 
						(
							SELECT DISTINCT
								LN90.BF_SSN,
								ABS(SUM(ISNULL(LN90.LA_FAT_NSI,0) + ISNULL(LN90.LA_FAT_CUR_PRI,0) + ISNULL(LN90.LA_FAT_LTE_FEE,0))) AS AMOUNT
							FROM
							--MAX_PST - MAX payment posted date within lookback period for active, non-reversed 1010 and 1035 payments
								(
									SELECT
										LN10.BF_SSN,
										LN10.LN_SEQ,
										MAX(LN90.LD_FAT_PST) AS MAX_LD_FAT_PST
									FROM 
										CDW..LN10_LON LN10
										INNER JOIN CDW..LN90_FIN_ATY LN90
											ON LN10.BF_SSN = LN90.BF_SSN
											AND LN10.LN_SEQ = LN90.LN_SEQ
									WHERE
										LN10.LA_CUR_PRI > 0.00 --open loans
										AND LN10.LC_STA_LON10 = 'R' --released loans
										AND LN90.LC_STA_LON90 = 'A' --active transactions
										AND ISNULL(LN90.LC_FAT_REV_REA,'') = '' --non-reversed transactions
										AND LN90.PC_FAT_TYP = '10' --1010 AND 1035 transactions
										AND LN90.PC_FAT_SUB_TYP IN ('10','35')
										AND LN90.LD_FAT_PST >= @LookBackDate --payment effective within target date range
									GROUP BY
										LN10.BF_SSN,
										LN10.LN_SEQ
								) MAX_PST
							--only include open, released, in repayment loans
								INNER JOIN CDW..LN10_LON LN10
									ON MAX_PST.BF_SSN = LN10.BF_SSN
									AND MAX_PST.LN_SEQ = LN10.LN_SEQ
								INNER JOIN CDW..DW01_DW_CLC_CLU DW01
									ON LN10.BF_SSN = DW01.BF_SSN
									AND LN10.LN_SEQ = DW01.LN_SEQ
									AND DW01.WC_DW_LON_STA = '03' -- in repayment
									AND ISNULL(DW01.WX_OVR_DW_LON_STA, '') = ''
							--payment information
								INNER JOIN CDW..LN90_FIN_ATY LN90
									ON LN10.BF_SSN = LN90.BF_SSN
									AND LN10.LN_SEQ = LN90.LN_SEQ
							--find the "current" bill as defined by the most recent bill sent
								INNER JOIN 
								(
									SELECT
										LN80.BF_SSN,
										LN80.LN_SEQ,
										MAX(LN80.LD_BIL_CRT) AS LD_BIL_CRT,
										MAX(LN80.LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
										MAX(LN80.LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
									FROM
										CDW..BL10_BR_BIL BL10
										INNER JOIN CDW..LN80_LON_BIL_CRF LN80
											ON BL10.BF_SSN = LN80.BF_SSN
											AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
											AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
									WHERE
										LN80.LC_STA_LON80 = 'A'
										AND LN80.LC_BIL_TYP_LON = 'P'
										AND BL10.LC_IND_BIL_SNT  IN ('1','G','R','2','7','4','F','I','H') -- bill sent
									GROUP BY
										LN80.BF_SSN,
										LN80.LN_SEQ
								) CUR_BILL
									ON LN10.BF_SSN = CUR_BILL.BF_SSN
									AND LN10.LN_SEQ = CUR_BILL.LN_SEQ
							WHERE
								LN10.LA_CUR_PRI > 0.00 --open loans
								AND LN10.LC_STA_LON10 = 'R' --released loans
								AND LN90.LC_STA_LON90 = 'A' --active transactions
								AND ISNULL(LN90.LC_FAT_REV_REA,'') = '' --non-reversed transactions
								AND LN90.PC_FAT_TYP = '10' --1010 AND 1035 transactions
								AND LN90.PC_FAT_SUB_TYP IN ('10','35')
								AND LN90.LD_FAT_PST BETWEEN CAST(CUR_BILL.LD_BIL_CRT AS DATE) AND MAX_PST.MAX_LD_FAT_PST
							GROUP BY
								LN90.BF_SSN
						) PMT
							ON LN10.BF_SSN = PMT.BF_SSN
						INNER JOIN
					--DUE calculate total amount due for all loans for current bill
						(
							SELECT DISTINCT
								LN80.BF_SSN,
								SUM(ISNULL(LN80.LA_BIL_CUR_DU,0) + ISNULL(LN80.LA_BIL_PAS_DU,0)) AS AMT_DUE
							FROM
							--MAX_PST - MAX payment posted date within lookback period for active, non-reversed 1010 and 1035 payments
								(
									SELECT
										LN10.BF_SSN,
										LN10.LN_SEQ,
										MAX(LN90.LD_FAT_PST) AS MAX_LD_FAT_PST
									FROM 
										CDW..LN10_LON LN10
										INNER JOIN CDW..LN90_FIN_ATY LN90
											ON LN10.BF_SSN = LN90.BF_SSN
											AND LN10.LN_SEQ = LN90.LN_SEQ
									WHERE
										LN10.LA_CUR_PRI > 0.00 --open loans
										AND LN10.LC_STA_LON10 = 'R' --released loans
										AND LN90.LC_STA_LON90 = 'A' --active transactions
										AND ISNULL(LN90.LC_FAT_REV_REA,'') = '' --non-reversed transactions
										AND LN90.PC_FAT_TYP = '10' --1010 AND 1035 transactions
										AND LN90.PC_FAT_SUB_TYP IN ('10','35')
										AND LN90.LD_FAT_PST >= @LookBackDate --payment effective within target date range
									GROUP BY
										LN10.BF_SSN,
										LN10.LN_SEQ
								) MAX_PST
							--only include open, released, in repayment loans
								INNER JOIN CDW..LN10_LON LN10
									ON MAX_PST.BF_SSN = LN10.BF_SSN
									AND MAX_PST.LN_SEQ = LN10.LN_SEQ
								INNER JOIN CDW..DW01_DW_CLC_CLU DW01
									ON LN10.BF_SSN = DW01.BF_SSN
									AND LN10.LN_SEQ = DW01.LN_SEQ
									AND DW01.WC_DW_LON_STA = '03' -- in repayment
									AND ISNULL(DW01.WX_OVR_DW_LON_STA, '') = ''
							--find the "current" bill as defined by the most recent bill sent
								INNER JOIN 
								(
									SELECT
										LN80.BF_SSN,
										LN80.LN_SEQ,
										MAX(LN80.LD_BIL_CRT) AS LD_BIL_CRT,
										MAX(LN80.LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
										MAX(LN80.LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
									FROM
										CDW..BL10_BR_BIL BL10
										INNER JOIN CDW..LN80_LON_BIL_CRF LN80
											ON BL10.BF_SSN = LN80.BF_SSN
											AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
											AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
									WHERE
										LN80.LC_STA_LON80 = 'A'
										AND LN80.LC_BIL_TYP_LON = 'P'
										AND BL10.LC_IND_BIL_SNT IN ('1','G','R','2','7','4','F','I','H') -- bill sent
									GROUP BY
										LN80.BF_SSN,
										LN80.LN_SEQ
								) CUR_BILL
									ON LN10.BF_SSN = CUR_BILL.BF_SSN
									AND LN10.LN_SEQ = CUR_BILL.LN_SEQ
							--join to LN80 for the "current" bill (see join above) to get the amount due for the bill
								INNER JOIN CDW..LN80_LON_BIL_CRF LN80
									ON CUR_BILL.BF_SSN = LN80.BF_SSN
									AND CUR_BILL.LN_SEQ = LN80.LN_SEQ
									AND CUR_BILL.LD_BIL_CRT = LN80.LD_BIL_CRT
									AND CUR_BILL.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
									AND CUR_BILL.LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
									AND LN80.LC_STA_LON80 = 'A'
									AND LN80.LC_BIL_TYP_LON = 'P'
							WHERE
								LN10.LA_CUR_PRI > 0.00 --open loans
								AND LN10.LC_STA_LON10 = 'R' --released loans
							GROUP BY
								LN80.BF_SSN
						) DUE
							ON PMT.BF_SSN = DUE.BF_SSN
					--borrower is delinquent
						INNER JOIN
						(
							SELECT
								BF_SSN,
								LN_SEQ,
								(LN_DLQ_MAX + 1) AS LN_DLQ_MAX_1
							FROM
								CDW..LN16_LON_DLQ_HST
							WHERE
								LC_STA_LON16 = '1' -- active delinquency
						) LN16
							ON LN10.BF_SSN = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
							AND LN16.LN_DLQ_MAX_1 >= 5
					--check for the existence of an active ARC to exclude borrowers who already have it
						LEFT JOIN
						(
							SELECT
								BF_SSN,
								CAST(MAX(LD_ATY_REQ_RCV) AS DATE) AS LD_ATY_REQ_RCV
							FROM
								CDW..AY10_BR_LON_ATY
							WHERE
								PF_REQ_ACT = @ARC
								AND LC_STA_ACTY10 = 'A'
							GROUP BY
								BF_SSN
						) AY10
							ON LN10.BF_SSN = AY10.BF_SSN
							AND AY10.LD_ATY_REQ_RCV >= MAX_PMT.MAX_LD_FAT_PST
					WHERE
						PMT.AMOUNT < DUE.AMT_DUE --payment amount is less than the amount due 
						AND AY10.BF_SSN IS NULL --exclude borrowers which already have an active underpayment ARC
				) NewData
			--exclude borrowers who already have an arcadd record
				LEFT JOIN CLS..ArcAddProcessing ExistingData
					ON NewData.DF_SPE_ACC_ID = ExistingData.AccountNumber
					AND ExistingData.ARC = @ARC
					AND (
							(
								CAST(ExistingData.CreatedAt AS DATE) =  CAST(@CurrentDateTime AS DATE) --to remove anyone added today to prevent duplicates in recovery
								AND ExistingData.Comment = 'Borrower from designated states made an underpayment on ' + COALESCE(RTRIM(CONVERT(VARCHAR(10),NewData.LD_FAT_PST,101)),'') + ' and is delinquent'
							)
							OR ExistingData.ProcessedAt IS NULL
						)
			WHERE
				ExistingData.AccountNumber IS NULL --exclude borrowers who already have an arcadd record
		) InsertData
	;

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
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;