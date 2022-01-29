/*
Issue:
For CR testing, we need to identify a few example accounts where borrowers have endorsers and where the borrower and/or endorser have made payments. There are four combinations we need to identify. 

X. Borrower type having "last payment" and cosigner type not having last payment.
X. Cosigner type having "last payment" and Borrower type not having last payment.
X. Borrower type and cosigner type having no "last payment".
X. Having more than one Cosigner.


X. Borrower type having "last payment" and cosigner type not having last payment. Identify borrowers for scenario one 

Table LNXX_FIN_ATY

WHERE LNXX.LD_FAT_EFF = most recent date 
AND LNXX.LC_STA_LONXX = 'A' 
AND LNXX.LC_FAT_REV_REA is null
AND LNXX.PC_FAT_TYP = 'XX'
AND LNXX.PC_SUB_FAT_TYP = 'XX'

Please only include top X borrowers and their endorsers in output.

X. WHERE LNXX.LD_FAT_EFF = most recent date 
AND LNXX.LC_STA_LONXX = 'A' 
AND LNXX.LC_FAT_REV_REA is null
AND LNXX.PC_FAT_TYP = 'XX'
AND LNXX.PC_SUB_FAT_TYP = 'XX'

Please only include top X borrowers and their endorsers in output.

X. WHERE LNXX.LD_FAT_EFF = null or blank (no payments have been made). Please only include X borrowers and their endorsers in output. 

X. Having more than one cosigner

LNXX_EDS
Where for one BF_SSN there are multiple entries in the LNXX table for fields LC_REL_TO_BR, LC_EDS_TYP and LF_EDS. Please include X examples if possible. */



SELECT * FROM OPENQUERY(LEGEND,
'
	SELECT
		PDXX.DF_SPE_ACC_ID,
		LNXX.BF_SSN,
		LNXX.LN_SEQ,
		CASE WHEN LNXX.EndrCount > X THEN ''Having more than one Cosigner.''
			 WHEN LNXX_X.BF_SSN IS NOT NULL AND LNXX_X.BF_SSN IS NOT NULL THEN
				CASE WHEN LNXX_X.LD_FAT_EFF >= LNXX_X.LD_FAT_EFF /*Determine whether borrower or endorser made most recent payment*/
					THEN ''Borrower type having "last payment" and cosigner type not having last payment.''
					ELSE ''Cosigner type having "last payment" and Borrower type not having last payment.'' 
				END
			 WHEN LNXX_X.BF_SSN IS NOT NULL AND LNXX_X.BF_SSN IS NULL THEN /*Borrower is only payer*/
				''Borrower type having "last payment" and cosigner type not having last payment.''
			 WHEN LNXX_X.BF_SSN IS NULL AND LNXX_X.BF_SSN IS NOT NULL THEN /*Cosigner is only payer*/
				''Cosigner type having "last payment" and Borrower type not having last payment.''
			 WHEN LNXX_X.BF_SSN IS NULL AND LNXX_X.BF_SSN IS NULL THEN /*No Payments*/
				''Borrower type and cosigner type having no "last payment".''
			 ELSE
				''N/A''
		END AS Grouping			
	FROM
		PKUB.PDXX_PRS_NME PDXX
		INNER JOIN PKUB.LNXX_LON LNXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		INNER JOIN 
		(	
			SELECT
				BF_SSN,
				LN_SEQ,
				COUNT(DISTINCT LF_EDS) AS EndrCount
			FROM
				PKUB.LNXX_EDS /*active endorser exists*/
			WHERE
				COALESCE(LC_REL_TO_BR,'''') != ''''
				AND COALESCE(LC_EDS_TYP,'''') != ''''
				AND COALESCE(LF_EDS,'''') != ''''
				AND LC_STA_LONXX = ''A''
			GROUP BY
				BF_SSN,
				LN_SEQ
		) LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		LEFT JOIN 
		(	
			SELECT
				BF_SSN,
				LN_SEQ,
				MAX(LD_FAT_EFF) AS LD_FAT_EFF
			FROM
				PKUB.LNXX_FIN_ATY
			WHERE
				LC_STA_LONXX = ''A''
				AND COALESCE(LC_FAT_REV_REA,'''') = ''''
				AND PC_FAT_TYP = ''XX''
				AND PC_FAT_SUB_TYP = ''XX''
			GROUP BY
				BF_SSN,
				LN_SEQ
		) LNXX_X
			ON LNXX_X.BF_SSN = LNXX.BF_SSN
			AND LNXX_X.LN_SEQ = LNXX.LN_SEQ
		LEFT JOIN 
		(	
			SELECT
				BF_SSN,
				LN_SEQ,
				MAX(LD_FAT_EFF) AS LD_FAT_EFF
			FROM
				PKUB.LNXX_FIN_ATY
			WHERE
				LC_STA_LONXX = ''A''
				AND COALESCE(LC_FAT_REV_REA,'''') = ''''
				AND PC_FAT_TYP = ''XX''
				AND PC_FAT_SUB_TYP = ''XX''
			GROUP BY
				BF_SSN,
				LN_SEQ
		) LNXX_X
			ON LNXX_X.BF_SSN = LNXX.BF_SSN
			AND LNXX_X.LN_SEQ = LNXX.LN_SEQ	
'
)