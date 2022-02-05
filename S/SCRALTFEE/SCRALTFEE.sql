
BEGIN TRY
BEGIN TRANSACTION

	DECLARE @Today DATE = CAST(GETDATE() AS DATE);
	DECLARE @ScraLateFeePop TABLE (AccountNumber CHAR(10), LoanSequence INT, NeedsSCRAL BIT, NeedsSCRAF BIT, Comment VARCHAR(200));
	DECLARE @ScriptId VARCHAR(9) = 'SCRALTFEE';
	DECLARE @ArcForOutstandingLateFee VARCHAR(5) = 'SCRAF';
	DECLARE @ArcForPaymentMade VARCHAR(5) = 'SCRAL';
	DECLARE @CommentForOutstandingLateFee VARCHAR(200) = 'SCRA Borrower with Outstanding Late Fee.';
	DECLARE @CommentForLateFeePaymentMade VARCHAR(200) = 'SCRA Borrower Payment made to cover late fee.';
	DECLARE @ArcLoan TABLE(ArcAddProcessingId INT, AccountNumber VARCHAR(10));
	DECLARE @BeginDate DATE = (SELECT CAST(ISNULL(MIN(CreatedAt), GETDATE()) AS DATE) FROM ULS..ArcAddProcessing WHERE ScriptId = @ScriptId); --Get first date this script ran

	IF (@BeginDate != @Today AND DATEDIFF(DAY, @BeginDate, @Today) > 7) --Establish lookback period for script to look over last week. Don't look back before script was promoted.
		SET @BeginDate = CAST(DATEADD(DAY, -7, GETDATE()) AS DATE)

	/* Gather SCRA borrowers who have late fees (SCRAF pop) */
	INSERT INTO @ScraLateFeePop (AccountNumber, LoanSequence, NeedsSCRAL, NeedsSCRAF, Comment)
	SELECT DISTINCT
		BillsAndFees.AccountNumber,
		BillsAndFees.LoanSequence,
		0 AS NeedsSCRAL,
		1 AS NeedsSCRAF,
		@CommentForOutstandingLateFee AS Comment
	FROM
	(
		SELECT 
			ScraPop.DF_SPE_ACC_ID AS AccountNumber,
			ScraPop.BF_SSN AS BorrowerSsn,
			ScraPop.LN_SEQ AS LoanSequence,
			LastBill.LD_BIL_CRT AS LastBillCreateDate
		FROM
		(
			SELECT DISTINCT
				LN72.BF_SSN,
				PD10.DF_SPE_ACC_ID,
				LN72.LN_SEQ,
				LN72.LD_ITR_EFF_BEG AS ScraStart,
				LN72.LD_ITR_EFF_END AS ScraEnd
			FROM
				UDW..LN72_INT_RTE_HST LN72
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = LN72.BF_SSN
			WHERE
				LN72.LC_STA_LON72 = 'A'
				AND 
					(
						LN72.LC_INT_RDC_PGM = 'M' --SCRA int reduction indicator. If M, then on SCRA.
						OR 
						(
							LN72.LC_INT_RDC_PGM = 'P' --SCRA int reduction indiciator, but on inf reduction program in one of the below reduction program types
							AND LN72.LC_INT_RDC_PGM_TYP IN ('A', 'C', 'E', 'H', 'W', 'X', 'Z')
						)
					)
		) ScraPop -- Population of borrowers & their loans currently on SCRA
		INNER JOIN
		(
			SELECT DISTINCT
				LN80.BF_SSN,
				LN80.LN_SEQ,
				LN80.LD_BIL_CRT,
				LN80.LN_SEQ_BIL_WI_DTE,
				LN80.LN_BIL_OCC_SEQ
			FROM 
				UDW..LN80_LON_BIL_CRF LN80
				INNER JOIN UDW..BL10_BR_BIL BL10
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT 
					AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
					AND BL10.LC_BIL_TYP = 'P'
				LEFT JOIN UDW..LN75_BIL_LON_FAT LN75
					ON LN75.BF_SSN = LN80.BF_SSN
					AND LN75.LN_SEQ = LN80.LN_SEQ
					AND LN75.LD_BIL_CRT = LN80.LD_BIL_CRT
					AND LN75.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
					AND LN75.LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
				LEFT JOIN UDW..LN90_FIN_ATY LN90
					ON LN90.BF_SSN = LN80.BF_SSN
					AND LN90.LN_SEQ = LN80.LN_SEQ
					AND LN90.LN_FAT_SEQ = LN75.LN_FAT_SEQ
					AND LN90.LC_STA_LON90 = 'A'
					AND ISNULL(LN90.LC_FAT_REV_REA,'') = ''
			WHERE
				BL10.LC_STA_BIL10 = 'A'
				AND LN80.LC_STA_LON80 = 'A'
				AND ISNULL(LN80.LA_LTE_FEE_OTS_PRT, 0.00) > 0.00 
				AND ISNULL(LN80.LC_LTE_FEE_WAV_REA, '') = '' --Late fee not waived
				AND LN90.LA_FAT_LTE_FEE IS NULL --No late fee payment made
		) LateFees -- Population of borrowers with late fees
			ON LateFees.BF_SSN = ScraPop.BF_SSN
			AND LateFees.LN_SEQ = ScraPop.LN_SEQ
			AND LateFees.LD_BIL_CRT BETWEEN ScraPop.ScraStart AND ScraPop.ScraEnd --Bill during SCRA eligibility period
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
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
					AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
			WHERE
				LN80.LC_STA_LON80 = 'A'
				AND LN80.LC_BIL_TYP_LON = 'P'
				AND BL10.LC_IND_BIL_SNT IN ('1','G','R','2','7','4','F','I','H') --bill sent
				AND LN80.LD_BIL_DU_LON <= DATEADD(DAY, 30, @Today) --Bill is within next 30 days
			GROUP BY
				LN80.BF_SSN,
				LN80.LN_SEQ
		) LastBill -- Filter late fee population to only late fees present on last bill
			ON LastBill.BF_SSN = ScraPop.BF_SSN
			AND LastBill.LN_SEQ = ScraPop.LN_SEQ
			AND LastBill.LD_BIL_CRT = LateFees.LD_BIL_CRT
			AND LastBill.LN_SEQ_BIL_WI_DTE = LateFees.LN_SEQ_BIL_WI_DTE
			AND LastBill.LN_BIL_OCC_SEQ = LateFees.LN_BIL_OCC_SEQ
		) BillsAndFees
		LEFT JOIN 
		(
			SELECT
				AY10.BF_SSN,
				LN85.LN_SEQ,
				MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY AY10
				INNER JOIN UDW..LN85_LON_ATY LN85
					ON LN85.BF_SSN = AY10.BF_SSN
					AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
			WHERE
				AY10.PF_REQ_ACT = @ArcForOutstandingLateFee
				AND AY10.LC_STA_ACTY10 = 'A'
			GROUP BY
				AY10.BF_SSN,
				LN85.LN_SEQ
		) ArcForLastBill -- Population of SCRAF ARCs that post-date last bill
			ON ArcForLastBill.BF_SSN = BillsAndFees.BorrowerSsn
			AND ArcForLastBill.LN_SEQ = BillsAndFees.LoanSequence
			AND ArcForLastBill.LD_ATY_REQ_RCV >= BillsAndFees.LastBillCreateDate
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = BillsAndFees.BorrowerSsn --Borrower level exclusion since BU will be reviewing account and should find all instances of fees
			AND WQ20.WC_STA_WQUE20 IN ('A','H','P','U','W') --Not closed or canceled
			AND WQ20.PF_REQ_ACT IN (@ArcForOutstandingLateFee, @ArcForPaymentMade) --If borrower has a queue task associated with either SCRAL or SCRAF
		WHERE
			ArcForLastBill.BF_SSN IS NULL -- Last bill did not already receive SCRAF comment
			AND WQ20.BF_SSN IS NULL --Excluding borrowers who already have an LF/WF queue task
			AND BillsAndFees.LastBillCreateDate >= @BeginDate

	/* Gather SCRA borrowers who have made payments on late fees (SCRAL pop) */
	INSERT INTO @ScraLateFeePop (AccountNumber, LoanSequence, NeedsSCRAL, NeedsSCRAF, Comment)
	SELECT DISTINCT
		PaymentsOnScra.AccountNumber,
		PaymentsOnScra.LoanSequence,
		1 AS NeedsSCRAL,
		0 AS NeedsScraf,
		@CommentForLateFeePaymentMade AS Comment
	FROM
	(
		SELECT 
			ScraPop.DF_SPE_ACC_ID AS AccountNumber,
			ScraPop.BF_SSN AS BorrowerSsn,
			ScraPop.LN_SEQ AS LoanSequence,
			PaymentsMade.EffectiveDate
		FROM
		(
			SELECT DISTINCT
				LN72.BF_SSN,
				PD10.DF_SPE_ACC_ID,
				LN72.LN_SEQ,
				LN72.LD_ITR_EFF_BEG AS ScraStart,
				LN72.LD_ITR_EFF_END AS ScraEnd
			FROM
				UDW..LN72_INT_RTE_HST LN72
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = LN72.BF_SSN
			WHERE
				LN72.LC_STA_LON72 = 'A'
				AND 
					(
						LN72.LC_INT_RDC_PGM = 'M' --SCRA int reduction indicator. If M, then on SCRA.
						OR 
						(
							LN72.LC_INT_RDC_PGM = 'P' --SCRA int reduction indiciator, but on inf reduction program in one of the below reduction program types
							AND LN72.LC_INT_RDC_PGM_TYP IN ('A', 'C', 'E', 'H', 'W', 'X', 'Z')
						)
					)
		) ScraPop -- Population of borrowers & their loans currently on SCRA
		INNER JOIN
		(
			SELECT DISTINCT
				LN90.BF_SSN AS BorrowerSsn,
				LN90.LN_SEQ AS LoanSequence,
				LN90.LD_FAT_EFF AS EffectiveDate
			FROM 
				UDW..LN90_FIN_ATY LN90
			WHERE
				LN90.LC_STA_LON90 = 'A'
				AND ISNULL(LN90.LC_FAT_REV_REA,'') = ''
				AND ABS(LN90.LA_FAT_LTE_FEE) > 0.00
				AND LN90.LD_FAT_EFF >= @BeginDate --Payment made within last week
		) PaymentsMade -- Population of borrowers who made payments on late fees
			ON PaymentsMade.BorrowerSsn = ScraPop.BF_SSN
			AND PaymentsMade.LoanSequence = ScraPop.LN_SEQ
			AND PaymentsMade.EffectiveDate BETWEEN ScraPop.ScraStart AND ScraPop.ScraEnd --Payment issued during SCRA eligibility period
	) PaymentsOnScra
	LEFT JOIN 
	(
		SELECT
			AY10.BF_SSN,
			LN85.LN_SEQ,
			MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
		FROM
			UDW..AY10_BR_LON_ATY AY10
			INNER JOIN UDW..LN85_LON_ATY LN85
				ON LN85.BF_SSN = AY10.BF_SSN
				AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
		WHERE
			AY10.PF_REQ_ACT = @ArcForPaymentMade
			AND AY10.LC_STA_ACTY10 = 'A'
		GROUP BY
			AY10.BF_SSN,
			LN85.LN_SEQ
	) ArcForLastPayment -- Population of SCRAL ARCs that post-date last late fee payment
		ON ArcForLastPayment.BF_SSN = PaymentsOnScra.BorrowerSsn
		AND ArcForLastPayment.LN_SEQ = PaymentsOnScra.LoanSequence
		AND ArcForLastPayment.LD_ATY_REQ_RCV >= PaymentsOnScra.EffectiveDate
	LEFT JOIN UDW..WQ20_TSK_QUE WQ20
		ON WQ20.BF_SSN = PaymentsOnScra.BorrowerSsn --Borrower level exclusion since BU will be reviewing account and should find all instances of payments
		AND WQ20.WC_STA_WQUE20 IN ('A','H','P','U','W') --Not closed or canceled
		AND WQ20.PF_REQ_ACT = @ArcForPaymentMade
	WHERE
		ArcForLastPayment.BF_SSN IS NULL -- Last payment did not already receive SCRAL comment
		AND WQ20.BF_SSN IS NULL --Excluding borrowers who already have a SCRAL-related queue task

--SELECT
--	*
--FROM
--	@ScraLateFeePop


	/* Add SCRAF & SCRAF ARCs into AAP */
	INSERT INTO ULS..ArcAddProcessing(ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, CreatedAt, CreatedBy)
	OUTPUT INSERTED.ArcAddProcessingId, INSERTED.AccountNumber INTO @ArcLoan(ArcAddProcessingId, AccountNumber)
	SELECT DISTINCT
		SLFP.ArcTypeId,
		SLFP.AccountNumber,
		SLFP.ARC,
		SLFP.ScriptId,
		SLFP.ProcessOn,
		SLFP.Comment,
		SLFP.IsReference,
		SLFP.IsEndorser,
		SLFP.CreatedAt,
		SLFP.CreatedBy
	FROM
	(
		SELECT 
			0 AS ArcTypeId, --Add by loan
			SCRA.AccountNumber,
			CASE
				WHEN SCRA.NeedsSCRAF = 1 THEN @ArcForOutstandingLateFee
				WHEN SCRA.NeedsSCRAL = 1 THEN @ArcForPaymentMade
				ELSE NULL
			END AS ARC,
			@ScriptId AS ScriptId,
			GETDATE() AS ProcessOn,
			CASE
				WHEN SCRA.NeedsSCRAF = 1 THEN @CommentForOutstandingLateFee
				WHEN SCRA.NeedsSCRAL = 1 THEN SCRA.Comment
				ELSE NULL
			END AS Comment,
			0 AS IsReference,
			0 AS IsEndorser,
			GETDATE() AS CreatedAt,
			SUSER_NAME() AS CreatedBy
		FROM
			@ScraLateFeePop SCRA
			LEFT JOIN @ScraLateFeePop SCRALPOP
				ON SCRALPOP.AccountNumber = SCRA.AccountNumber
				AND SCRALPOP.NeedsSCRAL = 1
		WHERE
			(
				SCRA.NeedsSCRAF = 1
				AND SCRALPOP.AccountNumber IS NULL --Has outstanding late fee, no payment made
			)
			OR SCRA.NeedsSCRAL = 1 --Late fee payment made
	) SLFP
	LEFT JOIN ULS..ArcAddProcessing AAP
		ON AAP.AccountNumber = SLFP.AccountNumber
		AND AAP.ARC = SLFP.ARC
		AND AAP.ScriptId = @ScriptId
		AND AAP.Comment = SLFP.Comment
		AND 
		(
			CAST(AAP.CreatedAt AS DATE) = @Today --Prevent same-day dupes
			OR AAP.ProcessedAt IS NULL --Matching record hasn't been processed yet
		)
	WHERE
		AAP.AccountNumber IS NULL


	/* Add AAP record into AAP loan selection */
	INSERT INTO ULS..ArcLoanSequenceSelection(ArcAddProcessingId, LoanSequence)
	SELECT
		AL.ArcAddProcessingId,
		SLFP.LoanSequence
	FROM
		@ScraLateFeePop SLFP
		INNER JOIN @ArcLoan AL
			ON AL.AccountNumber = SLFP.AccountNumber

COMMIT TRANSACTION;

END TRY
BEGIN CATCH
	DECLARE @ErrorMessage VARCHAR(4000) = 'Transaction not committed. The ' + @ScriptId + ' encountered an error. Error: ' + (SELECT ERROR_MESSAGE());
	PRINT @ErrorMessage
	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId, NotificationSeverityTypeId, ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId, @NotificationSeverityTypeId, @ProcessLogId, NULL, NULL);
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId, @ErrorMessage);

	THROW;
END CATCH;