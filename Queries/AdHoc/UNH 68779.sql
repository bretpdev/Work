--Can a Programmer get the Lender and Bond IDs for all the accounts in the RS/UT test region?
SELECT * --393579
	FROM
		OPENQUERY
			(
				QADBD004,
				'
					SELECT DISTINCT
						LN10.BF_SSN,
						LN10.LN_SEQ,
						LN10.LF_LON_CUR_OWN,
						LN35.IF_BND_ISS
					FROM
						OLWHRM1.LN10_LON LN10
						LEFT JOIN OLWHRM1.LN35_LON_OWN LN35
							ON LN35.BF_SSN = LN10.BF_SSN
							AND LN35.LN_SEQ = LN10.LN_SEQ
							AND LN35.IF_OWN = LN10.LF_LON_CUR_OWN
							AND LC_STA_LON35 = ''A''
				'
			)

