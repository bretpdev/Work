SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	LNXX.BF_SSN,
	LTRIM(RTRIM(PDXX.DM_PRS_X)) + ' ' + LTRIM(RTRIM(PDXX.DM_PRS_LST)) AS NAME,
	lkXX.PX_DSC_EX_LNG,
	CASE 
		WHEN PDXXH.DF_PRS_ID IS NOT NULL THEN PDXXH.DN_DOM_PHN_ARA + '-' + PDXXH.DN_DOM_PHN_XCH + '-' + PDXXH.DN_DOM_PHN_LCL
		ELSE '' 
	END AS HOME_PHONE,
	CASE 
		WHEN PDXXA.DF_PRS_ID IS NOT NULL THEN PDXXA.DN_DOM_PHN_ARA + '-' + PDXXA.DN_DOM_PHN_XCH + '-' + PDXXA.DN_DOM_PHN_LCL
		ELSE '' 
	END AS ALT_PHONE,
	CASE 
		WHEN PDXXW.DF_PRS_ID IS NOT NULL THEN PDXXW.DN_DOM_PHN_ARA + '-' + PDXXW.DN_DOM_PHN_XCH + '-' + PDXXW.DN_DOM_PHN_LCL
		ELSE '' 
	END AS WORK_PHONE,
	CASE 
		WHEN PDXXM.DF_PRS_ID IS NOT NULL THEN PDXXM.DN_DOM_PHN_ARA + '-' + PDXXM.DN_DOM_PHN_XCH + '-' + PDXXM.DN_DOM_PHN_LCL
		ELSE '' 
	END AS MOBILE_PHONE,
	CASE 
		WHEN PDXXH.DF_PRS_ID IS NOT NULL THEN PDXXH.DX_ADR_EML
		ELSE '' 
	END AS HOME_EMAIL,
	ISNULL(CONVERT(VARCHAR, AYXX_CALL.LD_ATY_REQ_RCV,XXX), '') AS LAST_DIALER_CALL,
	ISNULL(CONVERT(VARCHAR, AYXX_EMAIL.LD_ATY_REQ_RCV,XXX), '') AS LAST_EMAIL_SENT
	--MAX(COALESCE(NCHH.ActivityDate,NCHA.ActivityDate,NCHW.ActivityDate,NCHM.ActivityDate) ) OVER (PARTITION BY PDXX.DF_PRS_ID)
	
	

FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
		ON DWXX.BF_SSN = LNXX.BF_SSN
		AND DWXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER  JOIN
	(
		SELECT DISTINCT
			BF_SSN
		FROM
			CDW..LNXX_LON
		WHERE
			LF_DOE_SCL_ORG IN ('XXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX')

		UNION

		SELECT DISTINCT 
			LF_STU_SSN
		FROM 
			[CDW].[dbo].[SDXX_STU_SPR]
		WHERE 
			LF_DOE_SCL_ENR_CUR IN ('XXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX')

		UNION

		SELECT DISTINCT 
			BF_SSN
		FROM 
			CDW..DFXX_BR_DFR_REQ
		WHERE 
			LF_DOE_SCL_DFR IN ('XXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX')


	) SCH_POP
		ON SCH_POP.BF_SSN = PDXX.DF_PRS_ID
	LEFT JOIN CDW..PDXX_PRS_PHN PDXXH
		ON PDXXH.DF_PRS_ID = PDXX.DF_PRS_ID
		AND PDXXH.DC_PHN = 'H'
		AND PDXXH.DI_PHN_VLD = 'Y'
	LEFT JOIN CDW..PDXX_PRS_PHN PDXXA
		ON PDXXA.DF_PRS_ID = PDXX.DF_PRS_ID
		AND PDXXA.DC_PHN = 'A'
		AND PDXXA.DI_PHN_VLD = 'Y'
	LEFT JOIN CDW..PDXX_PRS_PHN PDXXW
		ON PDXXW.DF_PRS_ID = PDXX.DF_PRS_ID
		AND PDXXW.DC_PHN = 'W'
		AND PDXXW.DI_PHN_VLD = 'Y'
	LEFT JOIN CDW..PDXX_PRS_PHN PDXXM
		ON PDXXM.DF_PRS_ID = PDXX.DF_PRS_ID
		AND PDXXM.DC_PHN = 'M'
		AND PDXXM.DI_PHN_VLD = 'Y'
	LEFT JOIN CDW..PDXX_PRS_ADR_EML PDXXH
		ON PDXXH.DF_PRS_ID = PDXX.DF_PRS_ID
		AND PDXXH.DC_ADR_EML = 'H'
		AND PDXXH.DI_VLD_ADR_EML = 'Y'
	LEFT JOIN 
	(
		SELECT DISTINCT
			BF_SSN,
			MAX(LN_ATY_SEQ) AS LN_ATY_SEQ,
			MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
		FROM
			CDW..AYXX_BR_LON_ATY AYXX
		WHERE
			PF_REQ_ACT = 'DDPHN'
		GROUP BY
			AYXX.BF_SSN
	)AYXX_CALL
		ON AYXX_CALL.BF_SSN = PDXX.DF_PRS_ID
	LEFT JOIN 
	(
		SELECT DISTINCT
			BF_SSN,
			MAX(LN_ATY_SEQ) AS LN_ATY_SEQ,
			MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
		FROM
			CDW..AYXX_BR_LON_ATY AYXX
		WHERE
			PF_REQ_ACT IN ('ACHEM','CNCEM','DDEML','DIREM','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQEXX','DQSEM','EMXXX','EMBWR','EMLXX','ERPMT','EXFBD','FBREM','NDSNF','PCXXX','PCEXX','SCRAN','TXEML')
		GROUP BY
			AYXX.BF_SSN
	)AYXX_EMAIL
		ON AYXX_EMAIL.BF_SSN = PDXX.DF_PRS_ID
	--LEFT JOIN NobleCalls..NobleCallHistory NCHAN
	--	ON NCHAN.AccountIdentifier =  PDXX.DF_SPE_ACC_ID
	--LEFT JOIN NobleCalls..NobleCallHistory NCHH
	--	ON NCHH.PhoneNumber =  PDXXH.DN_DOM_PHN_ARA + PDXXH.DN_DOM_PHN_XCH  + PDXXH.DN_DOM_PHN_LCL
	--	AND NCHH.IsInbound = X
	--LEFT JOIN NobleCalls..NobleCallHistory NCHA
	--	ON NCHA.PhoneNumber =  PDXXA.DN_DOM_PHN_ARA + PDXXA.DN_DOM_PHN_XCH  + PDXXA.DN_DOM_PHN_LCL
	--	AND NCHA.IsInbound = X
	--LEFT JOIN NobleCalls..NobleCallHistory NCHW
	--	ON NCHW.PhoneNumber =  PDXXW.DN_DOM_PHN_ARA + PDXXW.DN_DOM_PHN_XCH  + PDXXW.DN_DOM_PHN_LCL
	--	AND NCHW.IsInbound = X
	--LEFT JOIN NobleCalls..NobleCallHistory NCHM
	--	ON NCHM.PhoneNumber =  PDXXM.DN_DOM_PHN_ARA + PDXXM.DN_DOM_PHN_XCH  + PDXXM.DN_DOM_PHN_LCL
	--	AND NCHM.IsInbound = X
	LEFT JOIN 
	(
		SELECT * FROM OPENQUERY(DUSTER, 'SELECT * FROM OLWHRMX.LKXX_LS_CDE_LKP WHERE PM_ATR = ''WC-DW-LON-STA''')
	) LKXX
		on LKXX.PX_ATR_VAL = DWXX.WC_DW_LON_STA
WHERE 
	LF_DOE_SCL_ORG IN ('XXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX')