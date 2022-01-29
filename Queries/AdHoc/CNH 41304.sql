--CNH XXXXX - IDR Forgiveness Compliance Review

--a positive principal current balance
--repayment start date prior to X/X/XXXX
--have ever been in any of the following plans: IC, CX, IL, IB, IA, IX.

--Requested output: 
--Borrower first & last Name
--SSN
--number of total forbearance months
---number of qualifying payments for IBR, ICR, REPAYE


SELECT DISTINCT
	PDXX.DM_PRS_X AS [First Name], --Borrower first Name
	PDXX.DM_PRS_LST AS [Last Name], --Borrower last Name
	LNXX.BF_SSN AS SSN, --SSN
	LNXX.LN_SEQ AS [Loan Sequence], --loan sequence
	FORB.FOR_MOS AS [Total Forbearance Months], --number of total forbearance months
	FGV_PGM_CD AS [Forgiveness Program Code],
	FGV_PGM as [Forgiveness Program],
	 QUAL_PMTS.PMTS AS [Qualifying Payments],	--number of qualifying payments for IBR, ICR, REPAYE
	LNXX.LC_TYP_SCH_DIS AS [Current Repayment Plan], --current repayment plan
	DWXX.WD_LON_RPD_SR AS [Repayment Start Date]
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
		ON LNXX.BF_SSN = DWXX.BF_SSN
		AND LNXX.LN_SEQ = DWXX.LN_SEQ
	INNER JOIN CDW..LNXX_LON_RPS PLANS --have ever been in any of the following plans: IC, CX, IL, IB, IA, IX.
		ON LNXX.BF_SSN = PLANS.BF_SSN
		AND LNXX.LN_SEQ = PLANS.LN_SEQ
		--no active flag to determine "Have ever been"
		AND PLANS.LC_TYP_SCH_DIS IN ('IC', 'CX', 'IL', 'IB', 'IA', 'IX')
	LEFT JOIN CDW..LNXX_LON_RPS LNXX -- current repayment plan
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LC_STA_LONXX = 'A' --current plan only
--FORB: total forbearance months
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			LN_SEQ,
			SUM(FOR_DAYS)/XX.XX AS FOR_MOS  --sum days for each forbearance and divide by XX to get decimal value number of months
		FROM
			( --calculate the number of days for each forbearance
				SELECT
					LNXX.BF_SSN,
					LNXX.LN_SEQ,
					LNXX.LF_FOR_CTL_NUM,
					LNXX.LN_FOR_OCC_SEQ,
					DATEDIFF(DAY,CAST(LNXX.LD_FOR_BEG AS DATE),CAST(LNXX.LD_FOR_END AS DATE)) AS FOR_DAYS
				FROM
					CDW..FBXX_BR_FOR_REQ FBXX
					INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
						ON FBXX.BF_SSN = LNXX.BF_SSN
						AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
				WHERE
					FBXX.LC_FOR_STA = 'A'
					AND FBXX.LC_STA_FORXX = 'A'
					AND LNXX.LC_STA_LONXX = 'A'
					AND LNXX.LC_FOR_RSP != 'XXX'
					--AND FBXX.LC_FOR_TYP --TODO: certain types or all forbearances?
					--AND FBXX.BF_SSN = 'XXXXXXXXX' --for testing
			) FOR_DAYS
		GROUP BY
			BF_SSN,
			LN_SEQ
	) FORB
		ON LNXX.BF_SSN = FORB.BF_SSN
		AND LNXX.LN_SEQ = FORB.LN_SEQ
	LEFT JOIN
--QUALPMTS: number of qualifying payments for IBR, ICR, REPAYE
	(
		SELECT
			BF_SSN,
			LN_SEQ,
			LC_LON_FGV_PGM AS FGV_PGM_CD,
			CASE WHEN LC_LON_FGV_PGM = 'X' THEN 'IBR' WHEN LC_LON_FGV_PGM = 'X' THEN 'ICR' ELSE 'REPAYE' END AS FGV_PGM,
			SUM(LN_PAY_PRE_QLF_PAY+LN_PAY_PSC_QLF_PAY+LN_OV_QLF_PAY) AS PMTS
		FROM 
			 CDW..LNPC_LFG_PAY_DTL
		WHERE
			LC_LON_FGV_PGM IN ('X','X','X') --X=IBR, X=ICR, X=PAYE, X=REPAYE    
		GROUP BY
			BF_SSN,
			LN_SEQ,
			LC_LON_FGV_PGM,
			CASE WHEN LC_LON_FGV_PGM = 'X' THEN 'IBR' WHEN LC_LON_FGV_PGM = 'X' THEN 'ICR' ELSE 'REPAYE' END
	) QUAL_PMTS
		ON LNXX.BF_SSN = QUAL_PMTS.BF_SSN
		AND LNXX.LN_SEQ = QUAL_PMTS.LN_SEQ
WHERE
	LNXX.LA_CUR_PRI > X.XX ---a positive principal current balance
	AND DWXX.WD_LON_RPD_SR < 'XXXX-XX-XX' ---repayment start date prior to X/X/XXXX
ORDER BY
	PDXX.DM_PRS_X,
	PDXX.DM_PRS_LST,
	LNXX.BF_SSN,
	LNXX.LN_SEQ