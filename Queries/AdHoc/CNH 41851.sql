USE CDW
GO

DROP TABLE IF EXISTS #DATA
DROP TABLE IF EXISTS #FINAL

SELECT 
	*,
DENSE_RANK() OVER (PARTITION BY BF_SSN, LOAN_GROUP ORDER BY LD_LON_X_DSB) AS SUB_UNSUB_LOAN_GROUP
INTO #DATA
FROM
(
SELECT
	BF_SSN,
	LN_SEQ,
	LD_LON_X_DSB,
	IC_LON_PGM,
	CASE 
		WHEN IC_LON_PGM IN ('DLSTFD', 'DLUNST') then X  
		WHEN IC_LON_PGM IN ('DLSCNS','DLUCNS') THEN X
		WHEN IC_LON_PGM IN ('DLSSPL', 'DLUSPL') THEN X
	END AS LOAN_GROUP

	
FROM
	CDW..LNXX_LON LNXX
WHERE
	LNXX.IC_LON_PGM IN 
	(
		'DLUNST',	
		'DLSTFD',
		'DLUSPL',	
		'DLSSPL',
		'DLUCNS',	
		'DLSCNS'
	)
	AND LA_CUR_PRI > X
	AND LC_STA_LONXX = 'R'

	and LNXX.PF_MAJ_BCH in 
	(
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX',
		'XXXXXXXXXX'
	)
) POP


SELECT
	*,
	MAX(LN_IBR_QLF_PAY_PCV) OVER (PARTITION BY BF_SSN, LOAN_GROUP, SUB_UNSUB_LOAN_GROUP) MX,
	MIN(LN_IBR_QLF_PAY_PCV) OVER (PARTITION BY BF_SSN, LOAN_GROUP, SUB_UNSUB_LOAN_GROUP) MN
	into #final
FROM
(
	SELECT
		D.*,
		LN_IBR_QLF_PAY_PCV,
		LN_IBR_FGV_MTH_CTR,
		LN_RPYE_FGV_MCT,
		LN_PYE_FGV_MTH_CTR,
		LN_ICR_FGV_MTH_CTR,
		COUNT(*) OVER (PARTITION BY D.BF_SSN, D.LD_LON_X_DSB) AS LG
	
	FROM
		#DATA D
		INNER JOIN CDW..LNXX_RPD_PIO_CVN LNXX
			ON LNXX.BF_SSN = D.BF_SSN
			AND LNXX.LN_SEQ = D.LN_SEQ
) POP




SELECT 
	F.BF_SSN,
	F.LN_SEQ, 
	F.IC_LON_PGM,
	F.LD_LON_X_DSB,
	F.LN_IBR_QLF_PAY_PCV
	--*
FROM #FINAL F
WHERE  ISNULL(MX,X) != ISNULL(MN,X)
ORDER BY BF_SSN--, LOAN_GROUP

SELECT
	COUNT(DISTINCT BF_SSN)
FROM
	#final
WHERE MX IS NOT NULL  and MN is not null AND MX != MN