--BEGIN Transfer in Borrower Count
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
IF OBJECT_ID('tempdb..#ARCs2ADD') IS NOT NULL
BEGIN
  DROP TABLE #ARCs2ADD
END

-- create temp table with AAP data to be added to CLS.ArcAddProcessing table
SELECT
	LN10.LN_SEQ,
	0 [ArcTypeId], -- by loan
	PD10.DF_SPE_ACC_ID [AccountNumber],
	'PMTHR' [ARC],
	'IDRFRGVNS' [ScriptId],
	GETDATE() [ProcessOn],
	CASE 
		WHEN LN09.max_LD_25_YR_FGV_PCV IS NULL THEN 'Transfer in borrower has no pre-conversion forgiveness count. '
		ELSE 'Transfer in borrower has no pre-conversion forgiveness count. IDR forgive start date: ' + CAST(LN09.MAX_LD_25_YR_FGV_PCV AS VARCHAR(20))
	END [Comment],
	0 [IsReference],
	0 [IsEndorser]
INTO
	#ARCs2ADD
FROM
	CDW..PD10_PRS_NME PD10
	INNER JOIN CDW..LN10_LON LN10 
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN CDW..DW01_DW_CLC_CLU DW01 
		ON DW01.BF_SSN = LN10.BF_SSN 
		AND DW01.LN_SEQ = LN10.LN_SEQ
	INNER JOIN CDW..LN90_FIN_ATY LN90 
		ON LN90.BF_SSN = LN10.BF_SSN 
		AND LN90.LN_SEQ = LN10.LN_SEQ
		AND	LN90.PC_FAT_TYP = '02' -- transfer in
		AND	LN90.LC_STA_LON90 = 'A' -- active
		AND	COALESCE(LN90.LC_FAT_REV_REA,'') = '' --non reversed payment
	INNER JOIN CDW..LN65_LON_RPS LN65 
		ON LN65.BF_SSN = LN10.BF_SSN 
		AND LN65.LN_SEQ = LN10.LN_SEQ
		AND	LN65.LC_STA_LON65 = 'A' -- active
		AND	LN65.LC_TYP_SCH_DIS IN ('CA', 'I5', 'C1', 'C2', 'C3', 'IB', 'I3', 'CP', 'IA', 'CQ', 'IL', 'IP')
	INNER JOIN
	( -- borrowers with a 0 IDR Forgiveness counter
		SELECT
			LN09.BF_SSN,
			LN09.LN_SEQ,
			MAX(LN09.LD_25_YR_FGV_PCV) OVER (PARTITION BY LN09.BF_SSN) AS MAX_LD_25_YR_FGV_PCV -- max LN09.LD_25_YR_FGV_PCV by borrower
		FROM
			CDW..LN09_RPD_PIO_CVN LN09
		WHERE
			ISNULL(LN09.LN_IBR_QLF_PAY_PCV, 0) = 0
			AND ISNULL(LN09.LN_ICR_ON_TME_PAY, 0) = 0
	) LN09 
		ON LN09.BF_SSN = LN10.BF_SSN 
		AND LN09.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	( -- borrowers with 'PMTHR' ARC
		SELECT DISTINCT
			AY10.BF_SSN,
			LN85.LN_SEQ
		FROM
			CDW..AY10_BR_LON_ATY AY10
			INNER JOIN CDW..LN85_LON_ATY LN85 
				ON LN85.BF_SSN = AY10.BF_SSN 
				AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
		WHERE
			AY10.PF_REQ_ACT = 'PMTHR'
			AND AY10.LC_STA_ACTY10 = 'A'
	) PMTHR 
		ON PMTHR.BF_SSN = LN10.BF_SSN 
		AND PMTHR.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT
			AY10.BF_SSN,
			LN85.LN_SEQ
		FROM
			CDW..AY10_BR_LON_ATY AY10
			INNER JOIN CDW..LN85_LON_ATY LN85 
				ON LN85.BF_SSN = AY10.BF_SSN 
				AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
		WHERE
			AY10.PF_REQ_ACT = 'NCDRV'
			AND AY10.LC_STA_ACTY10 = 'A'
	) NCDRV
		ON NCDRV.BF_SSN = LN10.BF_SSN 
		AND NCDRV.LN_SEQ = LN10.LN_SEQ
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
			LEFT(AAP.Comment, 60) = LEFT('Transfer in borrower has no pre-conversion forgiveness count.', 60) -- ensure first 60 characters match (comment is dynamic)
			AND	AAP.ScriptId = 'IDRFRGVNS'
			AND	AAP.ARC = 'PMTHR'
			AND	AAP.ArcTypeId = 0 --by loan type
			AND	CAST(AAP.CreatedAt AS DATE) <= CAST(GETDATE() AS DATE)
	) AAP
		ON AAP.AccountNumber = PD10.DF_SPE_ACC_ID
		AND AAP.LoanSequence = LN10.LN_SEQ
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND	PMTHR.BF_SSN IS NULL -- exclude those loans with the 'PMTHR' ARC
	AND NCDRV.BF_SSN IS NULl -- Exclude loans that have the 'NEW CONSOL IDR DOC REVIEW' arc
	AND	AAP.LoanSequence IS NULL -- prevent duplicate inserts in the event the script is run multiple times before a warehouse refresh completes

BEGIN TRANSACTION
DECLARE @ERROR INT

-- insert data into AAP

INSERT INTO
	CLS..ArcAddProcessing
(
	ArcTypeId,
	AccountNumber,
	ARC,
	ScriptId,
	ProcessOn,
	Comment,
	IsReference,
	IsEndorser
)
SELECT DISTINCT
	AD.ArcTypeId,
	AD.AccountNumber,
	AD.ARC,
	AD.ScriptId,
	AD.ProcessOn,
	AD.Comment,
	AD.IsReference,
	AD.IsEndorser
FROM
	#ARCs2ADD AD

SET @ERROR = @@ERROR


 --Insert Loan Sequences 

INSERT INTO CLS..ArcLoanSequenceSelection
(
	ArcAddProcessingId,
	LoanSequence
)
SELECT
	AAP.ArcAddProcessingId,
	AD.LN_SEQ
FROM
	#ARCs2ADD AD
	INNER JOIN CLS..ArcAddProcessing AAP 
		ON AAP.AccountNumber = AD.AccountNumber
WHERE
	LEFT(AAP.Comment, 60) = LEFT('Transfer in borrower has no pre-conversion forgiveness count.', 60) -- ensure first 60 characters match (comment is dynamic)
	AND	AAP.ScriptId = 'IDRFRGVNS'
	AND	AAP.ARC = 'PMTHR'
	AND	AAP.ArcTypeId = 0
	AND	CAST(AAP.CreatedAt AS DATE) <= CAST(GETDATE() AS DATE)
	AND	AAP.ProcessedAt IS NULL

SET @ERROR = @ERROR + @@ERROR

IF OBJECT_ID('tempdb..#ARCs2ADD') IS NOT NULL
BEGIN
  DROP TABLE #ARCs2ADD
END

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

--END Transfer in Borrower Count Portion