/*
Issue:
For CR testing, we need to identify a few example accounts where borrowers have endorsers and where the borrower and/or endorser have made payments. There are four combinations we need to identify. 

1. Borrower type having "last payment" and cosigner type not having last payment.
2. Cosigner type having "last payment" and Borrower type not having last payment.
3. Borrower type and cosigner type having no "last payment".
4. Having more than one Cosigner.


1. Borrower type having "last payment" and cosigner type not having last payment. Identify borrowers for scenario one 

Table LN90_FIN_ATY

WHERE LN90.LD_FAT_EFF = most recent date 
AND LN90.LC_STA_LON90 = 'A' 
AND LN90.LC_FAT_REV_REA is null
AND LN90.PC_FAT_TYP = '10'
AND LN90.PC_SUB_FAT_TYP = '10'

Please only include top 2 borrowers and their endorsers in output.

2. WHERE LN90.LD_FAT_EFF = most recent date 
AND LN90.LC_STA_LON90 = 'A' 
AND LN90.LC_FAT_REV_REA is null
AND LN90.PC_FAT_TYP = '10'
AND LN90.PC_SUB_FAT_TYP = '15'

Please only include top 2 borrowers and their endorsers in output.

3. WHERE LN90.LD_FAT_EFF = null or blank (no payments have been made). Please only include 2 borrowers and their endorsers in output. 

4. Having more than one cosigner

LN20_EDS
Where for one BF_SSN there are multiple entries in the LN20 table for fields LC_REL_TO_BR, LC_EDS_TYP and LF_EDS. Please include 2 examples if possible. */



SELECT * FROM OPENQUERY(DUSTER,
'
	SELECT
		PD10.DF_SPE_ACC_ID,
		LN10.BF_SSN,
		LN10.LN_SEQ,
		CASE WHEN LN20.EndrCount > 1 THEN ''Having more than one Cosigner.''
			 WHEN LN90_1.BF_SSN IS NOT NULL AND LN90_2.BF_SSN IS NOT NULL THEN
				CASE WHEN LN90_1.LD_FAT_EFF >= LN90_2.LD_FAT_EFF /*Determine whether borrower or endorser made most recent payment*/
					THEN ''Borrower type having "last payment" and cosigner type not having last payment.''
					ELSE ''Cosigner type having "last payment" and Borrower type not having last payment.'' 
				END
			 WHEN LN90_1.BF_SSN IS NOT NULL AND LN90_2.BF_SSN IS NULL THEN /*Borrower is only payer*/
				''Borrower type having "last payment" and cosigner type not having last payment.''
			 WHEN LN90_1.BF_SSN IS NULL AND LN90_2.BF_SSN IS NOT NULL THEN /*Cosigner is only payer*/
				''Cosigner type having "last payment" and Borrower type not having last payment.''
			 WHEN LN90_1.BF_SSN IS NULL AND LN90_2.BF_SSN IS NULL THEN /*No Payments*/
				''Borrower type and cosigner type having no "last payment".''
			 ELSE
				''N/A''
		END AS Grouping			
	FROM
		OLWHRM1.PD10_PRS_NME PD10
		INNER JOIN OLWHRM1.LN10_LON LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN 
		(	
			SELECT
				BF_SSN,
				LN_SEQ,
				COUNT(DISTINCT LF_EDS) AS EndrCount
			FROM
				OLWHRM1.LN20_EDS /*active endorser exists*/
			WHERE
				COALESCE(LC_REL_TO_BR,'''') != ''''
				AND COALESCE(LC_EDS_TYP,'''') != ''''
				AND COALESCE(LF_EDS,'''') != ''''
				AND LC_STA_LON20 = ''A''
			GROUP BY
				BF_SSN,
				LN_SEQ
		) LN20
			ON LN20.BF_SSN = LN10.BF_SSN
			AND LN20.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN 
		(	
			SELECT
				BF_SSN,
				LN_SEQ,
				MAX(LD_FAT_EFF) AS LD_FAT_EFF
			FROM
				OLWHRM1.LN90_FIN_ATY
			WHERE
				LC_STA_LON90 = ''A''
				AND COALESCE(LC_FAT_REV_REA,'''') = ''''
				AND PC_FAT_TYP = ''10''
				AND PC_FAT_SUB_TYP = ''10''
			GROUP BY
				BF_SSN,
				LN_SEQ
		) LN90_1
			ON LN90_1.BF_SSN = LN10.BF_SSN
			AND LN90_1.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN 
		(	
			SELECT
				BF_SSN,
				LN_SEQ,
				MAX(LD_FAT_EFF) AS LD_FAT_EFF
			FROM
				OLWHRM1.LN90_FIN_ATY
			WHERE
				LC_STA_LON90 = ''A''
				AND COALESCE(LC_FAT_REV_REA,'''') = ''''
				AND PC_FAT_TYP = ''10''
				AND PC_FAT_SUB_TYP = ''15''
			GROUP BY
				BF_SSN,
				LN_SEQ
		) LN90_2
			ON LN90_2.BF_SSN = LN10.BF_SSN
			AND LN90_2.LN_SEQ = LN10.LN_SEQ	
'
)