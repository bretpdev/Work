	SELECT
		*
	FROM
	(
	SELECT
		lnXX.*,
		ROW_NUMBER() OVER (PARTITION BY LNXX.BF_SSN, LNXX.LN_SEQ ORDER BY LD_STA_LONXX DESC) AS SEQ
	FROM
		CDW..LNXX_INT_RTE_HST LNXX
		INNER JOIN CDW..LNXX_LON LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
			AND LNXX.LA_CUR_PRI > X
			AND LNXX.LC_STA_LONXX = 'R'
	WHERE
		LNXX.LC_STA_LONXX = 'A'
		AND CAST(GETDATE() AS DATE) BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END
		and LC_INT_RDC_PGM = 'S'
		and LC_INT_RDC_PGM_TYP = 'R'
	) POP
