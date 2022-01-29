SELECT
	*
FROM
	OPENQUERY
	(  -- borrowers who have ever been on an IDR plan
		LEGEND,
		'
			SELECT
				LNXX.BF_SSN,
				LNXX.LN_SEQ,
				LNXX.LF_FED_CLC_RSK,
				FRXX.LD_LON_X_DSB_OLD,
				FRXX.LD_LON_X_DSB_NEW,
				FRXX.LF_FED_CLC_RSK_OLD,
				FRXX.LF_FED_CLC_RSK_NEW
			FROM
				PKUB.LNXX_LON LNXX
				LEFT JOIN PKUB.FRXX_VIT_CHG_HST FRXX ON FRXX.BF_SSN = LNXX.BF_SSN AND FRXX.LN_SEQ = LNXX.LN_SEQ
			WHERE
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X,X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ = X
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ = X
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X,X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ = X
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X,X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X,X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X,X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X)
				)
				OR
				(
					LNXX.BF_SSN = ''XXXXXXXXX''
					AND
					LNXX.LN_SEQ in (X,X)
				)
			ORDER BY
				LNXX.BF_SSN,
				LNXX.LN_SEQ
		'
	) L 




