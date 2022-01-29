SELECT DISTINCT
	*
FROM
(
	SELECT distinct
		PD.DF_SPE_ACC_ID,
		PD.BF_SSN,
		CASE WHEN LN16.LN_SEQ IS NOT NULL THEN 'Y' ELSE 'N' END AS PAST_DUE,
		ISNULL(LN16.LN_DLQ_MAX,0) AS LN_DLQ_MAX,
		CASE WHEN PMT.LN_SEQ IS NOT NULL THEN 'Y' ELSE 'N' END AS MADE_PMT,
		PMT.LD_FAT_EFF,
		pmt.PMT_AMT

		--P.LA_RPS_ISL
	FROM
		CDW..[CNH_36886_Past Due] PD
		INNER JOIN CDW..CNH_36886_PAYMENT_run3 P
			ON PD.BF_SSN = P.BF_SSN
			AND PD.LN_SEQ = P.LN_SEQ
		LEFT JOIN CDW..LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = PD.BF_SSN
			AND LN16.LN_SEQ = PD.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
		LEFT JOIN
		(
			SELECT	
				p.bf_ssn,
				p.LN_SEQ,
				max(LD_FAT_EFF) as LD_FAT_EFF,
				SUM(PMT_AMT) AS PMT_AMT
			FROM
			(
			SELECT DISTINCT
				ln90.bf_ssn,
				LN90.LN_SEQ,
				ln90.LD_FAT_EFF,
				ABS(ISNULL(LA_FAT_NSI,0)) + ABS(ISNULL(LA_FAT_CUR_PRI,0)) AS PMT_AMT
				--10.*
			FROM
				CDW..CNH_36886_PAYMENT_run3 C
				INNER JOIN CDW..LN90_FIN_ATY LN90
					ON LN90.BF_SSN = C.BF_SSN
					AND LN90.LN_SEQ = C.LN_SEQ
				INNER JOIN CDW..LN94_LON_PAY_FAT LN94
					ON LN90.BF_SSN = LN94.BF_SSN
					AND LN90.LN_SEQ = LN94.LN_SEQ
					AND LN90.LN_FAT_SEQ  = LN94.LN_FAT_SEQ 
				INNER JOIN CDW..RM10_RMT_BCH RM10
					ON RM10.LD_RMT_BCH_INI = LN94.LD_RMT_BCH_INI
					AND RM10.LC_RMT_BCH_SRC_IPT = LN94.LC_RMT_BCH_SRC_IPT
					AND RM10.LN_RMT_BCH_SEQ = LN94.LN_RMT_BCH_SEQ
			WHERE
				LN90.LC_STA_LON90 = 'A'
				AND COALESCE(LN90.LC_FAT_REV_REA, '') = ''
				AND LN90.PC_FAT_TYP = '10'
				AND RM10.LC_RMT_BCH_SRC_IPT != 'E'
				and ln90.LD_FAT_EFF >= '02/01/2019'

			) P
			group by
				p.bf_ssn,
				p.LN_SEQ
		) PMT
			ON PMT.BF_SSN = PD.BF_SSN
			AND PMT.LN_SEQ = PD.LN_SEQ
) P
 WHERE P.MADE_PMT = 'Y' --AND P.PAST_DUE = 'Y'