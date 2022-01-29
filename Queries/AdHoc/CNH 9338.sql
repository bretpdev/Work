USE CDW
GO

DECLARE @ALLBWRS TABLE 
(
	BF_SSN CHAR(X),
	LN_SEQ INT,
	HAS_IDR BIT
)

INSERT INTO @ALLBWRS
SELECT 
	*
FROM
	OPENQUERY(LEGEND,
	'SELECT DISTINCT
		LNXX.BF_SSN,
		LNXX.LN_SEQ,
		CASE
			WHEN LNXX.LC_TYP_SCH_DIS IN (''CA'', ''CL'', ''CP'',''CQ'', ''CX'', ''CX'', ''CX'', ''IA'', ''IB'', ''IL'', ''IP'', ''IX'', ''IX'') THEN X
			ELSE X
		END AS HAS_IDR
	FROM
		PKUB.LNXX_LON LNXX
		INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
			ON DWXX.BF_SSN = LNXX.BF_SSN
			AND DWXX.LN_SEQ = LNXX.LN_SEQ
		INNER JOIN PKUB.LNXX_LON_RPS LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
	WHERE
		DWXX.WC_DW_LON_STA = ''XX''
		AND LNXX.LC_STA_LONXX = ''A''
		AND LNXX.LD_CRT_LONXX < ''XX/XX/XXXX''')

SELECT
	'NUMBER OF LOANS ON IDR' AS TITLE,
	COUNT(*) AS [COUNT]
FROM
	@ALLBWRS
WHERE
	HAS_IDR = X

UNION ALL

SELECT
	'NUMBER OF LOANS IN REPAYMENT' AS TITLE,
	COUNT(*) AS [COUNT]
FROM
	@ALLBWRS

UNION ALL

SELECT
	'NUMBER OF BORROWERS ON IDR' AS TITLE,
	COUNT(DISTINCT BF_SSN) AS [COUNT]
FROM
	@ALLBWRS
WHERE
	HAS_IDR = X

UNION ALL

SELECT
	'NUMBER OF BORROWER IN REPAYMENT' AS TITLE,
	COUNT(DISTINCT BF_SSN) AS [COUNT]
FROM
	@ALLBWRS

	