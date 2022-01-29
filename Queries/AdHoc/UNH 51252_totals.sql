/*
For each lender ID provide the following summary numbers. Use the first six characters of LN10.LF-LON-CUR-OWN to assign a lender to a loan.

When referencing LN10, Use LN10-LON-EOM

Total $ Sub stafford = Grand total for all loans as determined below:
LN10.IC-LON-PGM = STFFRD
LN10.LA-CUR-PRI + LN10.LA-NSI-OTS

Total $ of Unsub Stafford = Grand total for all loans as determined below:
LN10.IC-LON-PGM = UNSTFD
LN10.LA-CUR-PRI + LN10.LA-NSI-OTS

Total $ of PLUS = Grand total for all loans as determined below:
LN10.IC-LON-PGM = PLUS, PLUSGB
LN10.LA-CUR-PRI + LN10.LA-NSI-OTS

Total $ of CONSOL = Grand total for all loans as determined below:
LN10.IC-LON-PGM = CONSOL, CNSLDN, SPCNSL, SUBCNS, SUBSPC, UNCNS, UNSPC
LN10.LA-CUR-PRI + LN10.LA-NSI-OTS

Also, for each loan capture the LN10.IF-GTR and provide a unique list of all guarantors associated with these loans.
*/

SELECT
	*
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				LEFT(LN10.LF_LON_CUR_OWN, 6) "LF_LON_CUR_OWN",
				SUM(CASE WHEN LN10.IC_LON_PGM = ''STFFRD'' THEN (COALESCE(LN10.LA_CUR_PRI, 0) + COALESCE(LN10.LA_NSI_OTS, 0)) ELSE 0 END) "SubStafford",
				SUM(CASE WHEN LN10.IC_LON_PGM = ''UNSTFD'' THEN (COALESCE(LN10.LA_CUR_PRI, 0) + COALESCE(LN10.LA_NSI_OTS, 0)) ELSE 0 END) "UnsubStafford",
				SUM(CASE WHEN LN10.IC_LON_PGM = ''PLUS'' THEN (COALESCE(LN10.LA_CUR_PRI, 0) + COALESCE(LN10.LA_NSI_OTS, 0)) ELSE 0 END) "PLUS",
				SUM(CASE WHEN LN10.IC_LON_PGM = ''PLUSGB'' THEN (COALESCE(LN10.LA_CUR_PRI, 0) + COALESCE(LN10.LA_NSI_OTS, 0)) ELSE 0 END) "PLUSGB",
				SUM(CASE WHEN LN10.IC_LON_PGM IN (''CONSOL'', ''CNSLDN'', ''SPCNSL'', ''SUBCNS'', ''SUBSPC'', ''UNCNS'', ''UNSPC'') THEN (COALESCE(LN10.LA_CUR_PRI, 0) + COALESCE(LN10.LA_NSI_OTS, 0)) ELSE 0 END) "CONSOL"
			FROM
				OLWHRM1.LN10_LON_EOM LN10
			WHERE
				LN10.LC_STA_LON10 = ''R''
				AND
				LEFT(LN10.LF_LON_CUR_OWN, 6) IN (''826717'',''828476'',''830248'',''829769'')
			GROUP BY
				LEFT(LN10.LF_LON_CUR_OWN, 6)
		'
	) 

SELECT
	*
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				LN10.BF_SSN,
				LN10.LN_SEQ,
				LEFT(LN10.LF_LON_CUR_OWN, 6) "LF_LON_CUR_OWN",
				CASE WHEN LN10.IC_LON_PGM = ''STFFRD'' THEN (COALESCE(LN10.LA_CUR_PRI, 0) + COALESCE(LN10.LA_NSI_OTS, 0)) ELSE 0 END "SubStafford",
				CASE WHEN LN10.IC_LON_PGM = ''UNSTFD'' THEN (COALESCE(LN10.LA_CUR_PRI, 0) + COALESCE(LN10.LA_NSI_OTS, 0)) ELSE 0 END "UnsubStafford",
				CASE WHEN LN10.IC_LON_PGM = ''PLUS'' THEN (COALESCE(LN10.LA_CUR_PRI, 0) + COALESCE(LN10.LA_NSI_OTS, 0)) ELSE 0 END "PLUS",
				CASE WHEN LN10.IC_LON_PGM IN (''CONSOL'', ''CNSLDN'', ''SPCNSL'', ''SUBCNS'', ''SUBSPC'', ''UNCNS'', ''UNSPC'') THEN (COALESCE(LN10.LA_CUR_PRI, 0) + COALESCE(LN10.LA_NSI_OTS, 0)) ELSE 0 END "CONSOL",
				LN10.IF_GTR
			FROM
				OLWHRM1.LN10_LON_EOM LN10
			WHERE
				LN10.LC_STA_LON10 = ''R''
				AND
				LEFT(LN10.LF_LON_CUR_OWN, 6) IN (''826717'',''828476'',''830248'',''829769'')
		'
	)


SELECT
	*
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT DISTINCT
				LN10.IF_GTR
			FROM
				OLWHRM1.LN10_LON_EOM LN10
			WHERE
				LN10.LC_STA_LON10 = ''R''
				AND
				LEFT(LN10.LF_LON_CUR_OWN, 6) IN (''826717'',''828476'',''830248'',''829769'')
		'
	) 