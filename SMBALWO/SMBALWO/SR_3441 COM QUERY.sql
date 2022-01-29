DECLARE @TILP VARCHAR (2) = '!='
DECLARE @SQL VARCHAR(MAX) = 'SELECT * FROM OPENQUERY(DUSTER,
''
			SELECT
				LN10.BF_SSN,
				LN10.LN_SEQ,
				TOTAL_BAL.BAL,
				MAX(CASE
						WHEN (LN90.LC_STA_LON90 = ''''A'''' AND LN90.PC_FAT_TYP = ''''10'''' AND LN90.PC_FAT_SUB_TYP IN (''''70'''',''''80'''')) THEN ''''CONSOLIDATED''''
						ELSE ''''NOT CONSOLIDATED''''
					END
					) AS STATUS,
				ABS(MAX(CASE
						WHEN LN90.LC_CSH_ADV = ''''C'''' THEN  DAYS(LN90.LD_FAT_APL) - DAYS(CURRENT_DATE) 
						ELSE NULL
					END 
					)) AS  LD_FAT_APL_DAYS
			FROM
				OLWHRM1.LN10_LON LN10
				INNER JOIN
				(
					SELECT
						LN10.BF_SSN,
						SUM((LN10.LA_CUR_PRI + LN10.LA_NSI_OTS)) AS BAL
					FROM
						OLWHRM1.LN10_LON LN10
					WHERE
						LN10.IC_LON_PGM '+ @TILP +' ''''TILP''''
					GROUP BY
						LN10.BF_SSN
				) TOTAL_BAL
					ON LN10.BF_SSN = TOTAL_BAL.BF_SSN
				INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
					ON LN10.BF_SSN = DW01.BF_SSN
					AND LN10.LN_SEQ = DW01.LN_SEQ
					AND DW01.WC_DW_LON_STA != ''''23''''
				INNER JOIN
				(
					SELECT
						LN15.BF_SSN,
						LN15.LN_SEQ,
						SUM((LN15.LA_DSB - LN15.LA_DSB_CAN)) DSB_AMT
					FROM
						OLWHRM1.LN15_DSB LN15
					GROUP BY
						LN15.BF_SSN,
						LN15.LN_SEQ
				)LN15
					ON LN15.BF_SSN = LN10.BF_SSN
					AND LN15.LN_SEQ = LN10.LN_SEQ
				LEFT JOIN OLWHRM1.LN90_FIN_ATY LN90
					ON LN90.BF_SSN = LN10.BF_SSN
					AND LN90.LN_SEQ = LN10.LN_SEQ
			WHERE
				LN10.LC_STA_LON10 = ''''R''''
				AND (LN10.LA_CUR_PRI + LN10.LA_NSI_OTS) BETWEEN 0.01 AND 25
				AND (LN10.LA_CUR_PRI + LN10.LA_NSI_OTS) != LN15.DSB_AMT
				AND TOTAL_BAL.BAL < 25
				AND LN10.IC_LON_PGM '+ @TILP +' ''''TILP''''
			GROUP BY
				LN10.BF_SSN,
				LN10.LN_SEQ,
				TOTAL_BAL.BAL

'') OP
	WHERE
		(OP.STATUS = ''CONSOLIDATED'' AND OP.BAL < 25)
		OR
		(
			(OP.STATUS = ''NOT CONSOLIDATED'' AND OP.LD_FAT_APL_DAYS > 54 AND OP.BAL BETWEEN 25 AND 10)
			OR
			(OP.STATUS = ''NOT CONSOLIDATED'' AND OP.LD_FAT_APL_DAYS > 20 AND OP.BAL <= 10)
		)
'
SELECT @SQL
EXEC(@SQL)