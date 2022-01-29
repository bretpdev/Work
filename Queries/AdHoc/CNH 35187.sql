USE CDW
GO


SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT DISTINCT
	PDXX.DF_PRS_ID,
	PDXX.DF_SPE_ACC_ID,
	pdXX.DM_PRS_X,
	PDXX.DM_PRS_LST,
	PDXX.DD_DTH_NTF,
	FORB.LC_FOR_TYP,
	FORB.LD_FOR_BEG,
	FORB.LD_FOR_END,
	forb.LD_FOR_APL,
	LKXX.PX_DSC_EX_LNG
FROM
	CDW..PDXX_PRS_DTH PDXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON pdXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
		ON DWXX.BF_SSN = PDXX.DF_PRS_ID
	LEFT JOIN 
	(
		SELECT * FROM OPENQUERY(DUSTER, 'SELECT * FROM OLWHRMX.LKXX_LS_CDE_LKP WHERE PM_ATR = ''WC-DW-LON-STA''')
	) LKXX
		ON LKXX.PX_ATR_VAL = DWXX.WC_DW_LON_STA
	LEFT JOIN
	(
		SELECT DISTINCT
			LNXX.BF_SSN,
			FBXX.LC_FOR_TYP,
			LNXX.LD_FOR_BEG,
			LNXX.LD_FOR_END,
			LNXX.LD_FOR_APL
			--*
		FROM 
			CDW..LNXX_BR_FOR_APV LNXX
			INNER JOIN CDW..FBXX_BR_FOR_REQ FBXX
				ON FBXX.BF_SSN = LNXX.BF_SSN
				AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
		WHERE
			FBXX.LC_FOR_STA = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND FBXX.LC_STA_FORXX = 'A'
	) FORB
		ON FORB.BF_SSN = PDXX.DF_PRS_ID
		AND FORB.LD_FOR_BEG >= PDXX.DD_DTH_NTF
	LEFT JOIN
	(
		SELECT
			BF_SSN
		FROM
			CDW..AYXX_BR_LON_ATY
		WHERE
			PF_REQ_ACT = 'ADDTH'
	) AYXX
		ON AYXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	AYXX.BF_SSN IS NULL
ORDER BY
	PDXX.DF_PRS_ID
