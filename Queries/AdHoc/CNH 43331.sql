USE CDW;
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DROP TABLE IF EXISTS #LNXX_DSB, #POP;

--get original balance
SELECT --DISTINCT
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	SUM(ISNULL(LNXX.LA_DSB,X)) - SUM(ISNULL(LNXX.LA_DSB_CAN,X)) AS OriginalLoanBalance
INTO
	#LNXX_DSB
FROM
	LNXX_DSB LNXX
	INNER JOIN LNXX_LON LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE
	LNXX.LC_DSB_TYP = 'X'
	AND LNXX.LC_STA_LONXX IN ('X','X')
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'
GROUP BY
	LNXX.BF_SSN,
	LNXX.LN_SEQ
;
--select count(*) from #LNXX_DSB

--preliminary output:
SELECT DISTINCT
	PDXX.DM_PRS_LST AS BorrowerLastName,
	LNXX.BF_SSN AS BorrowerSSN,
	LNXX.LN_SEQ AS LoanSeq,
	ISNULL(LNXX.LA_CUR_PRI,X) + ISNULL(DWXX.WA_TOT_BRI_OTS,X) AS CurrentLoanBalance,
	ISNULL(LNXX.LA_CUR_PRI,X) AS Principal,
	ISNULL(DWXX.WA_TOT_BRI_OTS,X) AS Interest,
	LNXX_CALC.OriginalLoanBalance,
	--LNXX.LA_LON_AMT_GTR AS 'Original Loan Balance',  --JG this is the guaranteed amount, which isn�t the same as the amount that was actually disbursed.  The Original Balance should be calculated from LNXX, Grab actual released disbursements (LC_DSB_TYP = X, LC_STA_LONXX = X,X); sum the LA_DSB amounts and then subtract the LA_DSB_CAN cancellation amount. 	
	(ISNULL(LNXX.LA_FAT_CUR_PRI,X) + ISNULL(LNXX.LA_FAT_NSI,X)) AS PaymentAmount,
	--LNXX.LA_TOT_BIL_STS AS 'Payment Amount', --use to determine if payment went toward RPS bill  --JG WE SHOULD UPDATE TO USE LNXX FOR PAYMENT AMOUNT, SUM THE AMOUNTS THAT WENT TOWARDS PRINCIPAL AND INTEREST (LA_FAT_CUR_PRI + LA_FAT_NSI).  AS IS ACCT XX XXXX XXXX IS LISTING $XX.XX FOR LOAN X'S XX/XX/XXXX PAYMENT BUT THAT AMOUNT WAS REALLY $XXX.XX  I THINK IF WE USE LNXX IT�LL CORRECT THE ISSUE.
	LNXX.LD_FAT_EFF AS PaymentDate,
	PDXX.DC_DOM_ST AS StateOfResidence
INTO
	#POP
FROM
	PDXX_PRS_NME PDXX
	INNER JOIN PDXX_PRS_ADR PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN LNXX_LON_RPS LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN #LNXX_DSB LNXX_CALC
		ON LNXX.BF_SSN = LNXX_CALC.BF_SSN
		AND LNXX.LN_SEQ = LNXX_CALC.LN_SEQ
	INNER JOIN LNXX_LON_BIL_CRF LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
	INNER JOIN BLXX_BR_BIL BLXX
		ON LNXX.BF_SSN = BLXX.BF_SSN
		AND LNXX.LD_BIL_CRT = BLXX.LD_BIL_CRT
		AND LNXX.LN_SEQ_BIL_WI_DTE = BLXX.LN_SEQ_BIL_WI_DTE
	INNER JOIN LNXX_BIL_LON_FAT LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LD_BIL_CRT = LNXX.LD_BIL_CRT
		AND LNXX.LN_SEQ_BIL_WI_DTE = LNXX.LN_SEQ_BIL_WI_DTE
		AND LNXX.LN_BIL_OCC_SEQ = LNXX.LN_BIL_OCC_SEQ
	INNER JOIN LNXX_FIN_ATY LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		--AND LNXX.LN_FAT_SEQ = LNXX.LN_FAT_SEQ --commented out to get all payments made by loan
	INNER JOIN DWXX_DW_CLC_CLU DWXX
		ON LNXX.BF_SSN = DWXX.BF_SSN
		AND LNXX.LN_SEQ = DWXX.LN_SEQ
WHERE
	LNXX.LC_TYP_SCH_DIS  IN ('CX','CX','CX','CA','IX','IB','IX') 
	--AND LNXX.LC_STA_LONXX = 'A' --commented out because need inactive schedules
	AND LNXX.LC_STA_LONXX = 'R'
	AND ISNULL(LNXX.LA_CUR_PRI,X) + ISNULL(DWXX.WA_TOT_BRI_OTS,X) > X.XX
	AND PDXX.DC_ADR = 'L' --legal
	--AND CONVERT(DATE,LNXX.LD_CRT_LONXX) >= CONVERT(DATE,'XXXXXXXX') --JG COMMENTING OUT BECAUSE THE IDR SCHEDULE COULD HAVE BEEN CREATED BEFORE XX/XX/XXXX
	AND LNXX.LC_STA_LONXX = 'A'
	AND LNXX.LA_TOT_BIL_STS > X.XX --use to determine if payment went toward RPS bill
	AND BLXX.LC_STA_BILXX = 'A'
	AND BLXX.LC_BIL_TYP = 'P' --RPS bill
	AND LNXX.LD_FAT_EFF >= CONVERT(DATE,'XXXXXXXX')
	AND (LNXX.LC_FAT_REV_REA = '' or LNXX.LC_FAT_REV_REA IS NULL ) --not reversed  --JG ADDED NULL... FOR SOME REASON AES USES BOTH VALUES TO DESIGNATE NON-REVERSED TRANSACTIONS
	AND LNXX.LC_STA_LONXX = 'A'  --JG ADDED THIS LINE SO WE ONLY GRAB ACTIVE TRANSACTIONS
	AND LNXX.PC_FAT_TYP = 'XX'
;

--detail output:
SELECT
	*
FROM 
	#POP 
ORDER BY 
	--BorrowerSSN,
	BorrowerLastName,
	LoanSeq, 
	PaymentDate
;

--summary output:
SELECT DISTINCT
	B.BorrowerLastName,
	B.BorrowerSSN,
	COUNT(B.LoanSeq) AS 'Total Loan Count',
	SUM(B.Principal) AS 'Total Principal (all loans)',
	SUM(B.Interest) AS 'Total Interest (all loans)',
	SUM(B.CurrentLoanBalance)  AS 'Total Current Balance (all loans)',
	SUM(B.OriginalLoanBalance) AS 'Total Beginning Balance (all loans)',
	P.sum_PaymentAmount AS 'Total Payments (total of all payments made between X/XX/XXXX and current date)',
	B.StateOfResidence
FROM
	(--balances
		SELECT DISTINCT
			BorrowerLastName,
			BorrowerSSN,
			LoanSeq,
			Principal,
			Interest,
			CurrentLoanBalance,
			OriginalLoanBalance,
			StateOfResidence
		FROM
			#POP
	) B
	INNER JOIN
	(--payments
		SELECT DISTINCT
			BorrowerLastName,
			BorrowerSSN,
			SUM(PaymentAmount) AS sum_PaymentAmount,
			StateOfResidence
		FROM
			#POP
		GROUP BY
			BorrowerLastName,
			BorrowerSSN,
			StateOfResidence
	) P
		ON P.BorrowerLastName	= B.BorrowerLastName
		AND P.BorrowerSSN		= B.BorrowerSSN
		AND P.StateOfResidence = B.StateOfResidence
GROUP BY
	B.BorrowerLastName,
	B.BorrowerSSN,
	B.StateOfResidence,
	P.sum_PaymentAmount
ORDER BY
	B.BorrowerLastName,
	B.BorrowerSSN
;