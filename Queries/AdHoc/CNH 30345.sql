SELECT
	DF_PRS_ID
	,DM_PRS_X
	,DM_PRS_LST
	,MAX(LN_DLQ_MAX) AS LN_DLQ_MAX
	,DX_STR_ADR_X
	,DX_STR_ADR_X
	,DM_CT
	,DC_DOM_ST
	,DF_ZIP_CDE
	,DN_DOM_PHN_ARA
	,DN_DOM_PHN_XCH
	,DN_DOM_PHN_LCL
	,DX_CNC_EML_ADR
FROM OPENQUERY (LEGEND,'
	SELECT DISTINCT
		PDXX.DF_PRS_ID
		,PDXX.DM_PRS_X
		,PDXX.DM_PRS_LST
		,LNXX.LN_DLQ_MAX 
		,PDXX.DX_STR_ADR_X
		,PDXX.DX_STR_ADR_X
		,PDXX.DM_CT
		,PDXX.DC_DOM_ST
		,PDXX.DF_ZIP_CDE
		,PDXX.DN_DOM_PHN_ARA
		,PDXX.DN_DOM_PHN_XCH
		,PDXX.DN_DOM_PHN_LCL
		,PHXX.DX_CNC_EML_ADR
	FROM
		PKUB.PDXX_PRS_NME PDXX
		INNER JOIN PKUB.PDXX_PRS_ADR PDXX
			ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		INNER JOIN PKUB.PDXX_PRS_PHN PDXX
			ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		INNER JOIN PKUB.LNXX_LON LNXX 
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
		INNER JOIN 
		(	
			SELECT DISTINCT
				BF_SSN
				,LN_SEQ
				,MAX(LN_DLQ_MAX) AS LN_DLQ_MAX
			FROM
				PKUB.LNXX_LON_DLQ_HST
			GROUP BY
				BF_SSN
				,LN_SEQ
		)LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		INNER JOIN AES.PHXX_CNC_EML PHXX
			ON PDXX.DF_SPE_ACC_ID = PHXX.DF_SPE_ID
	WHERE
		(
			PDXX.DI_VLD_ADR = ''N''
			OR PDXX.DI_PHN_VLD = ''N''
		)
		AND LNXX.LN_DLQ_MAX BETWEEN X AND XXX
')
GROUP BY
	DF_PRS_ID
	,DM_PRS_X
	,DM_PRS_LST
	,DX_STR_ADR_X
	,DX_STR_ADR_X
	,DM_CT
	,DC_DOM_ST
	,DF_ZIP_CDE
	,DN_DOM_PHN_ARA
	,DN_DOM_PHN_XCH
	,DN_DOM_PHN_LCL
	,DX_CNC_EML_ADR
