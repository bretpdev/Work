USE CDW
GO

DECLARE @DATA TABLE ( NUMBER INT, SERVICER VARCHAR(XXX), SSN VARCHAR(X), CEMS_CASE_NUMBER VARCHAR(XX), [TYPE] VARCHAR(XX), CASE_OPENED_DATE DATE, BORROWER_NOTIFICATION_DATE DATE, DAYS_OF_INTEREST_CREDIT INT)
INSERT INTO @DATA VALUES
(X,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','XXX'),
(X,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','XXX'),
(X,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','XXX'),
(X,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','XXX'),
(X,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','XXX'),
(X,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','XXX'),
(X,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','XXX'),
(X,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','XXX'),
(X,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','XXX'),
(XX,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','X'),
(XX,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','XXX'),
(XX,'CS','XXXXXXXXX','XXXXXXXX','Ineligible','XX/XX/XX','XX/XX/XX','XXX')


SELECT DISTINCT
	D.*,
	'',
	LNXX.LN_SEQ,
	LNXX.LR_INT_RDC_PGM_ORG as LR_ITR,
	LNXX.LA_CUR_PRI,
	--'' AS ITSXR,
	CAST(ROUND(((LNXX.LR_INT_RDC_PGM_ORG / XXX) * LNXX.LA_CUR_PRI / XXX) * D.DAYS_OF_INTEREST_CREDIT, X,X) AS DECIMAL(XX,X))  INT_CALCULATOR,
	FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(LN_FED_AWD_SEQ AS VARCHAR(X)), X) AS AWARD_ID
	--'' AS TOTAL_INT_TO_ADJ_ITSXR
	--CASE WHEN LNXX.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N' END AS MADE_PAYMENT
FROM
	CDW..LNXX_INT_RTE_HST LNXX
	INNER JOIN @DATA D
		ON D.SSN = LNXX.BF_SSN
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..FSXX_DL_LON FSXX
		ON FSXX.BF_SSN = LNXX.BF_SSN
		AND FSXX.LN_SEQ = LNXX.LN_SEQ
	--LEFT JOIN CDW..LNXX_FIN_ATY LNXX
	--	ON LNXX.BF_SSN = LNXX.BF_SSN
	--	AND LNXX.LN_SEQ = LNXX.LN_SEQ
	--	AND LNXX.LD_FAT_EFF >= D.CASE_OPENED_DATE
	--	AND LNXX.LC_STA_LONXX = 'A'
	--	AND ISNULL(LNXX.LC_FAT_REV_REA,'') = ''
	--	AND LNXX.PC_FAT_TYP = 'XX'
WHERE
	LNXX.LC_STA_LONXX = 'A'
	AND CAST(GETDATE() AS DATE) BETWEEN LNXX.LD_ITR_EFF_BEG AND LD_ITR_EFF_END
ORDER BY
	d.NUMBER,
	LNXX.LN_SEQ