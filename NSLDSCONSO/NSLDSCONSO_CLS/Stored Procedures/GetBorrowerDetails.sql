CREATE PROCEDURE [nsldsconso].[GetBorrowerDetails]
	@BorrowerId INT
AS

	SELECT
		BUL.BorrowerUnderlyingLoanId,
		BUL.NsldsLabel,
		BUL.LoanType,
		BUL.UnderlyingLoanId,
		BUL.NewLoanId,
		BUL.FirstDisbursement,
		SUM(BULF.TotalAmount) TotalAmount,
		MAX(BULF.DateFunded) DateFunded
	FROM
		nsldsconso.BorrowerUnderlyingLoans BUL
	LEFT JOIN
		nsldsconso.BorrowerUnderlyingLoanFunding BULF ON BULF.BorrowerId = BUL.BorrowerId AND BULF.UnderlyingLoanId = BUL.UnderlyingLoanId AND BULF.LoanType = BUL.LoanType
	WHERE
		BUL.BorrowerId = @BorrowerId
	GROUP BY
		BUL.BorrowerUnderlyingLoanId, BUL.NsldsLabel, BUL.LoanType, BUL.UnderlyingLoanId, BUL.NewLoanId, BUL.FirstDisbursement



	SELECT DISTINCT
		BUL.BorrowerUnderlyingLoanId,
		BUL.BorrowerId,
		BUL.NewLoanId,
		BUL.FirstDisbursement,
		BUL.UnderlyingLoanId,
		BUL.NsldsLabel,
		BUL.LoanType,
		BUL.UnderlyingLoanBalance,
		SUM(BULF.TotalAmount) TotalAmount,
		MAX(BULF.DateFunded) DateFunded
	FROM
	(
		SELECT DISTINCT 
			BULA.BorrowerUnderlyingLoanId,
			BULA.BorrowerId,
			BULA.NewLoanId,
			BULA.FirstDisbursement,
			BULA.UnderlyingLoanId,
			BULA.NsldsLabel,
			BULA.LoanType,
			BULA.UnderlyingLoanBalance
		FROM
			nsldsconso.BorrowerUnderlyingLoans BULA
		LEFT JOIN
			nsldsconso.BorrowerUnderlyingLoans BULB
				ON BULA.BorrowerId = BULB.BorrowerId
				AND BULA.UnderlyingLoanId = BULB.UnderlyingLoanId
				AND BULA.LoanType = BULB.LoanType
				AND BULA.BorrowerUnderlyingLoanId != BULB.BorrowerUnderlyingLoanId
		WHERE
			BULB.BorrowerUnderlyingLoanId IS NOT NULL
	) BUL
	INNER JOIN 
		nsldsconso.BorrowerUnderlyingLoanFunding BULF
			ON BUL.BorrowerId = BULF.BorrowerId
			AND BUL.UnderlyingLoanId = BULF.UnderlyingLoanId
			AND BUL.LoanType = BULF.LoanType
			AND BUL.UnderlyingLoanBalance = BULF.TotalAmount
	WHERE
		BUL.BorrowerId = @BorrowerId		
	GROUP BY
		BUL.BorrowerUnderlyingLoanId, BUL.BorrowerId, BUL.NewLoanId, BUL.FirstDisbursement, BUL.UnderlyingLoanId,BUL.NsldsLabel, BUL.LoanType, BUL.UnderlyingLoanBalance

	UNION

	SELECT DISTINCT
		BULA.BorrowerUnderlyingLoanId,
		BULA.BorrowerId,
		BULA.NewLoanId,
		BULA.FirstDisbursement,
		BULA.UnderlyingLoanId,
		BULA.NsldsLabel,
		BULA.LoanType,
		BULA.UnderlyingLoanBalance,
		SUM(BULF.TotalAmount) TotalAmount,
		MAX(BULF.DateFunded) DateFunded
	FROM
		nsldsconso.BorrowerUnderlyingLoans BULA
	LEFT JOIN
		nsldsconso.BorrowerUnderlyingLoans BULB
			ON BULA.BorrowerId = BULB.BorrowerId
			AND BULA.UnderlyingLoanId = BULB.UnderlyingLoanId
			AND BULA.LoanType = BULB.LoanType
			AND BULA.BorrowerUnderlyingLoanId != BULB.BorrowerUnderlyingLoanId
	INNER JOIN
		nsldsconso.BorrowerUnderlyingLoanFunding BULF
			ON BULA.BorrowerId = BULF.BorrowerId
			AND BULA.UnderlyingLoanId = BULF.UnderlyingLoanId
			AND BULA.LoanType = BULF.LoanType
	WHERE 
		BULB.UnderlyingLoanId IS NULL
		AND
		BULA.BorrowerId = @BorrowerId
	GROUP BY
		BULA.BorrowerUnderlyingLoanId, BULA.BorrowerId, BULA.NewLoanId, BULA.FirstDisbursement, BULA.UnderlyingLoanId, BULA.NsldsLabel, BULA.LoanType, BULA.UnderlyingLoanBalance



	SELECT
		BCL.NewLoanId,
		BCL.GrossAmount,
		BCL.InterestRate,
		FS10.LN_SEQ [Fs10LoanSequence]
	FROM
		nsldsconso.BorrowerConsolidationLoans BCL
	JOIN
		nsldsconso.Borrowers B on B.BorrowerId = BCL.BorrowerId
	LEFT JOIN 
		CDW.dbo.FS10_DL_LON FS10 ON FS10.BF_SSN = B.Ssn AND (FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ  AS VARCHAR(MAX)),3))= BCL.NewLoanId 
	WHERE
		BCL.BorrowerId = @BorrowerId


	SELECT
		GRSP.IC_LON_PGM [LoanProgram],
		GRSP.LF_NDS_LON_RSP_LAB [NsldsLabel],
		GRSP.LF_FED_AWD [AwardId],
		GRSP.LN_FED_AWD_SEQ [AwardSequence]
	FROM
		CDW.dbo.GRSP_NDS_LON_RSP GRSP
	INNER JOIN
		nsldsconso.Borrowers B ON b.Ssn = GRSP.DF_PRS_ID
	WHERE
		B.BorrowerId = @BorrowerId


	SELECT
		GRS2.LD_DSB [DisbursementDate],
		GRS2.LN_LIS_DSB_SEQ [DisbursementSequence],
		GRS2.LF_NDS_LON_RSP_LAB [NsldsLabel]
	FROM
		CDW.dbo.GRS2_NDS_DSB_RSP GRS2
	INNER JOIN
		nsldsconso.Borrowers B ON B.Ssn = GRS2.DF_PRS_ID
	WHERE
		B.BorrowerId = @BorrowerId


	SELECT
		SUM(BULF.TotalAmount) MaxTotalAmount
	FROM
		nsldsconso.BorrowerUnderlyingLoanFunding BULF
	WHERE
		BULF.BorrowerId = @BorrowerId



RETURN 0
