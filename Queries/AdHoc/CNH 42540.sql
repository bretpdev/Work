/*Issue:
We need a query ran for all Cornerstone accounts that have had interest capitalizations on their loans since X/X/XXXX.

Include all loan types.

Include if a letter to the borrower was generated notifying of the interest cap in the output.

Output: letter generated, SSN, date of interest cap

Please have completed by COB X/XX/XXXX*/

SELECT DISTINCT
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	LNXX.LD_FAT_EFF AS DATE_OF_CAP,
	Letters.CreateDate,
	Letters.Letter
FROM
	CDW..LNXX_FIN_ATY LNXX
	LEFT JOIN
	(
		SELECT DISTINCT
			L.Letter,
			DD.Ssn,
			DD.CreateDate
		FROM
			EcorrFed..DocumentDetails DD
			INNER JOIN EcorrFed..Letters L
				ON L.LetterId = DD.LetterId
		WHERE
			L.Letter IN('TSXXBCAP','USXXBCAP','INTCAPCAS','INCPCAFED','INTCAPARS','INCPARFED')
			AND CAST(CreateDate AS DATE) >= 'XXXX-XX-XX'
	) Letters
		ON Letters.Ssn = LNXX.BF_SSN
WHERE
	LNXX.PC_FAT_TYP = 'XX'
	AND LNXX.LC_STA_LONXX = 'A'
	AND ISNULL(LNXX.LC_FAT_REV_REA,'') = ''
	AND CAST(LNXX.LD_FAT_EFF AS DATE) >= 'XXXX-XX-XX'
	AND ISNULL(Letters.CreateDate,'XXXX-XX-XX') >= CAST(LNXX.LD_FAT_EFF AS DATE)
ORDER BY
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	LNXX.LD_FAT_EFF
