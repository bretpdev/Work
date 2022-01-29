--To be clear, we need a list of the lender/bond ID combinations for open loans.  not a list of all loans with the lender/bond fields.
SELECT * 
	FROM
		OPENQUERY
			(
				QADBD004,
				'
					SELECT DISTINCT
						LN10.LF_LON_CUR_OWN,
						LN35.IF_BND_ISS
					FROM
						OLWHRM1.LN10_LON LN10
						LEFT JOIN OLWHRM1.LN35_LON_OWN LN35
							ON LN35.BF_SSN = LN10.BF_SSN
							AND LN35.LN_SEQ = LN10.LN_SEQ
							AND LN35.IF_OWN = LN10.LF_LON_CUR_OWN
							AND LC_STA_LON35 = ''A''
						WHERE
							LN10.LA_CUR_PRI > 0.00
							AND LN10.LC_STA_LON10 = ''R''
							AND LN10.LA_CUR_PRI > 0.00
							AND CAST(LD_OWN_EFF_SR AS DATE) <= CURRENT_DATE
							AND LD_OWN_EFF_END IS NULL

				'
			)

