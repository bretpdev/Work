IF OBJECT_ID('tempdb..#ReportData', 'U') IS NOT NULL
BEGIN
	DROP TABLE #ReportData
END

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

-- SELECT Report DATA
SELECT
	RS.BF_SSN,
	RS.LN_SEQ,
	RS.LD_RPS_1_PAY_DU [1stPaymentDue],
	RS.LC_TYP_SCH_DIS,
	RS.TotalDfrMonthsUsed,
	RS.TotalFrbMonthsUsed,
	RS.TotalDfrMonthsUsed + RS.TotalFrbMonthsUsed [TotalMonthsUsed],
	MIA.MonthlyAccruedInterest,
	RS.LA_RPS_ISL [MonthlyPayment],
	RS.LN_GRD_RPS_SEQ [CurrentGradation],
	RS.LA_CUR_PRI [OutstandingPrincipal],
	DW01.WA_TOT_BRI_OTS [OutstandingInterest],
	RS.GradationMonths,
	CASE WHEN MIA.LD_LON_ACL_ADD >= '2014-12-14' THEN 1 ELSE 0 END [ARCInstead] -- Add ARC instead of showing on report
INTO
	#ReportData
FROM
	CDW.calc.RepaymentSchedules RS
	INNER JOIN
	( -- Monthly Interest Accrual
		SELECT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			LN10.LD_LON_ACL_ADD,
			CAST(LN72.LR_ITR * LN10.LA_CUR_PRI / 100 / 365.25 * 30 AS MONEY) [MonthlyAccruedInterest]
		FROM
			CDW..LN10_LON LN10
			INNER JOIN CDW..LN72_INT_RTE_HST LN72 ON LN72.BF_SSN = LN10.BF_SSN AND LN72.LN_SEQ = LN10.LN_SEQ
		WHERE
			GETDATE() BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
			AND
			LN72.LC_STA_LON72 = 'A'
	) MIA ON MIA.BF_SSN = RS.BF_SSN AND MIA.LN_SEQ = RS.LN_SEQ
	INNER JOIN CDW..DW01_DW_CLC_CLU DW01 ON DW01.BF_SSN = RS.BF_SSN AND DW01.LN_SEQ = RS.LN_SEQ
WHERE
	RS.CurrentGradation = 1
	AND
	MIA.MonthlyAccruedInterest > RS.LA_RPS_ISL
	AND
	MIA.LD_LON_ACL_ADD >= '2014-12-14'
	AND
	RS.LC_TYP_SCH_DIS NOT IN ('IB', 'IL', 'I3', 'IP', 'C1', 'C2', 'C3', 'CL', 'CQ', 'CA', 'CP', 'IA', 'I5', 'RE')
ORDER BY
	RS.BF_SSN,
	RS.LN_SEQ,
	RS.LN_GRD_RPS_SEQ
	

BEGIN TRANSACTION
	
	DECLARE @ERROR INT
	
	-- Add ARC through ARC Add Processing
	INSERT INTO
		CLS..ArcAddProcessing
	(
		[ArcTypeId],
		[AccountNumber],
		[ARC],
		[ScriptId],
		[ProcessOn],
		[Comment],
		[IsReference],
		[IsEndorser]
	)
	SELECT DISTINCT
		0 [ArctypeId], --Add ARC by sequence number
		RD.BF_SSN,
		'GRSEL',
		'NOIDRNGAM',
		GETDATE(),
		'Non IDR Negam, please redisclose',
		0,
		0
	FROM
		#ReportData RD
		LEFT JOIN 
		( -- prevent duplicate inserts
			SELECT
				AccountNumber,
				ALSS.LoanSequence
			FROM
				CLS..ArcAddProcessing AAP 
				INNER JOIN CLS..ArcLoanSequenceSelection ALSS
					ON ALSS.ArcAddProcessingId = AAP.ArcAddProcessingId 
			WHERE
			AAP.Comment = 'Non IDR Negam, please redisclose'
			AND
			AAP.ScriptId = 'NOIDRNGAM'
			AND
			AAP.ARC = 'GRSEL'
			AND
			AAP.ArcTypeId = 0
			AND
			CAST(AAP.CreatedAt AS DATE) = CAST(GETDATE() AS DATE) -- don't add a second one for the same day
				AND	CAST(AAP.CreatedAt AS DATE) <= CAST(GETDATE() AS DATE)
		) AAP ON AAP.AccountNumber = RD.BF_SSN AND AAP.LoanSequence = RD.LN_SEQ
		LEFT JOIN CDW..AY10_BR_LON_ATY AY10 --Dont add another if there is a pending one already
			ON AY10.BF_SSN = RD.BF_SSN
			AND 
			AY10.PF_REQ_ACT = 'GRSEL'
			AND 
			AY10.PF_RSP_ACT IS NULL
	WHERE
		AAP.LoanSequence IS NULL
		AND 
		AY10.BF_SSN IS NULL 

	
	SET @ERROR = @ERROR + @@ERROR

	-- Insert Loan Sequences
	INSERT INTO CLS..ArcLoanSequenceSelection
	(
		ArcAddProcessingId,
		LoanSequence
	)
	SELECT
		AAP.ArcAddProcessingId,
		RD.LN_SEQ
	FROM
		#ReportData RD
		INNER JOIN CLS..ArcAddProcessing AAP ON AAP.AccountNumber = RD.BF_SSN
	WHERE
		AAP.Comment = 'Non IDR Negam, please redisclose'
		AND
		AAP.ScriptId = 'NOIDRNGAM'
		AND
		AAP.ARC = 'GRSEL'
		AND
		AAP.ArcTypeId = 0
		AND
		CAST(AAP.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
		AND
		AAP.ProcessedAt IS NULL

	SET @ERROR = @ERROR + @@ERROR

IF @ERROR != 0
	BEGIN
		PRINT '!!! ERROR - The transaction was rolled back.'
		RAISERROR('!!! ERROR - The transaction was rolled back.', 16, 1)
		ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'The transaction was committed.'
		COMMIT TRANSACTION
	END

IF OBJECT_ID('tempdb..#ReportData', 'U') IS NOT NULL
BEGIN
	DROP TABLE #ReportData
END
