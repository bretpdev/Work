
--Borrower SSN - LNXX.BF_SSN
--Loan seq - LNXX.LN_SEQ
--Original School Code - LNXX.LF_DOE_SCL_ORG
--Original School name - 
--School City - 
--School State - 
--Enrollment Begin (for the listed school) - SDXX.LD_ENR_BEG where SDXX.IF_DOE_SCL = LNXX.LF_DOE_SCL_ORG and SDXX.LC_STA_SDXX = A
--Enrollment end (for the listed school) - SDXX.LD_ENR_END where SDXX.IF_DOE_SCL = LNXX.LF_DOE_SCL_ORG and SDXX.LC_STA_SDXX = A


SELECT DISTINCT
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	LNXX.LF_DOE_SCL_ORG,
	SCXX.IM_SCL_FUL,
	SCXX.IM_SCL_CT,
	SCXX.IC_SCL_DOM_ST,
	SCXX.IF_SCL_ZIP_CDE,
	SDXX.BeginDate,
	SDXX.EndDate,
	LNXX.LD_TRM_BEG,
	LNXX.LD_TRM_END
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..SCXX_SCH_DMO SCXX
		ON SCXX.IF_DOE_SCL = LNXX.LF_DOE_SCL_ORG
	INNER JOIN CDW..SCXX_SCH_DPT SCXX
		ON SCXX.IF_DOE_SCL = LNXX.LF_DOE_SCL_ORG
		AND SCXX.IC_SCL_DPT = 'XXX'
	INNER JOIN
	(
		SELECT
			SDXX.LF_STU_SSN,
			SDXX.LF_DOE_SCL_ENR_CUR,
			MIN(LD_ENR_STA_EFF_CAM) AS BeginDate,
			MAX(LD_SCL_SPR) AS EndDate
		FROM
			CDW..SDXX_STU_SPR SDXX
		GROUP BY
			SDXX.LF_STU_SSN,
			SDXX.LF_DOE_SCL_ENR_CUR
	) SDXX
		ON SDXX.LF_STU_SSN = COALESCE(LNXX.LF_STU_SSN,LNXX.BF_SSN)
		AND SDXX.LF_DOE_SCL_ENR_CUR = LNXX.LF_DOE_SCL_ORG
		
WHERE
	LNXX.LF_DOE_SCL_ORG IN ('XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX')