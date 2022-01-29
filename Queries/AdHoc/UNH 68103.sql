/*Issue:
We need a query ran for all Cornerstone accounts that have had interest capitalizations on their loans since 7/1/2020.

Include all loan types.

Include if a letter to the borrower was generated notifying of the interest cap in the output.

Output: letter generated, SSN, date of interest cap

Please have completed by COB 7/27/2020*/

SELECT DISTINCT
	LN90.BF_SSN,
	LN90.LN_SEQ,
	LN90.LD_FAT_EFF AS DATE_OF_CAP,
	Letters.CreateDate,
	Letters.Letter
FROM
	UDW..LN90_FIN_ATY LN90
	LEFT JOIN
	(
		SELECT DISTINCT
			L.Letter,
			DD.Ssn,
			DD.CreateDate
		FROM
			EcorrUheaa..DocumentDetails DD
			INNER JOIN EcorrUheaa..Letters L
				ON L.LetterId = DD.LetterId
		WHERE
			L.Letter IN('TS06BCAP','US06BCAP','INTCAPCAS','INCPCAFED','INTCAPARS','INCPARFED')
			AND CAST(CreateDate AS DATE) >= '2020-07-01'
	) Letters
		ON Letters.Ssn = LN90.BF_SSN
WHERE
	LN90.PC_FAT_TYP = '70'
	AND LN90.LC_STA_LON90 = 'A'
	AND ISNULL(LN90.LC_FAT_REV_REA,'') = ''
	AND CAST(LN90.LD_FAT_EFF AS DATE) >= '2020-07-01'
	AND ISNULL(Letters.CreateDate,'2099-01-01') >= CAST(LN90.LD_FAT_EFF AS DATE)
ORDER BY
	LN90.BF_SSN,
	LN90.LN_SEQ,
	LN90.LD_FAT_EFF
