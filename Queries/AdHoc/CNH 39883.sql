/****** Script for SelectTopNRows command from SSMS  ******/
SELECT  [Loan Information: Record Number]
      ,[Borrower Name]
      ,[Borrower SSN]
      ,[Award ID/Loan ID]
      ,LNXX.LA_CUR_PRI,
	  ISNULL(LKXXDW.PX_DSC_MDM, DWXX.WC_DW_LON_STA) AS LOAN_STATUS,
	  CASE 
		WHEN FORB.LN_SEQ IS NOT NULL THEN LKXXF.PX_DSC_LNG
	    WHEN DEF.LN_SEQ IS NOT NULL THEN LKXXD.PX_DSC_LNG 
	  ELSE '' END AS FORB_DEF_TYPE,
	 ISNULL(CONVERT(VARCHAR(XX), COALESCE(FORB.LD_FOR_BEG, DEF.LD_DFR_BEG), XXX),'') AS DEF_FORB_BEG,
	  ISNULL( CONVERT(VARCHAR(XX), COALESCE(FORB.LD_FOR_END, DEF.LD_DFR_END),XXX),'') AS DEF_FORB_END,
	   ISNULL(RS.LC_TYP_SCH_DIS,'') AS REPAYMENT_PLAN,
	   ISNULL(CONVERT(VARCHAR(XX),RS.LA_RPS_ISL),'') AS REPAYMENT_AMT
  FROM [CDW].[dbo].['CNH XXXXX DATA$'] CNH
  INNER JOIN CDW..FSXX_DL_LON FSXX
	ON CNH.[Borrower SSN] = FSXX.BF_SSN
	AND CNH.[Award ID/Loan ID] = (FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)), X))
INNER JOIN CDW..LNXX_LON LNXX
	ON LNXX.BF_SSN = FSXX.BF_SSN
	AND LNXX.LN_SEQ = FSXX.LN_SEQ
INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
	ON DWXX.BF_SSN = FSXX.BF_SSN
	AND DWXX.LN_SEQ = FSXX.LN_SEQ
LEFT JOIN
(
	SELECT	DISTINCT
		FBXX.BF_SSN,
		LNXX.LN_SEQ,
		FBXX.LC_FOR_TYP ,
		max(LNXX.LD_FOR_BEG) as LD_FOR_BEG,
		max(LNXX.LD_FOR_END) as LD_FOR_END
	FROM	
		CDW..FBXX_BR_FOR_REQ FBXX
		INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
			ON LNXX.BF_SSN = FBXX.BF_SSN
			AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
	WHERE
		FBXX.LC_FOR_STA = 'A'
		AND LNXX.LC_STA_LONXX = 'A'
		AND FBXX.LC_STA_FORXX = 'A'
		--AND FBXX.LC_FOR_TYP = 'XX'
		AND GETDATE() BETWEEN LNXX.LD_FOR_BEG AND LNXX.LD_FOR_END
	group by fbXX.bf_ssn,
	FBXX.LC_FOR_TYP ,
	LNXX.LN_SEQ
)FORB
	ON FORB.BF_SSN = FSXX.BF_SSN
	AND FORB.LN_SEQ = FSXX.LN_SEQ
LEFT JOIN
(
	SELECT	DISTINCT
		FBXX.BF_SSN,
		LNXX.LN_SEQ,
		FBXX.LC_DFR_TYP ,
		max(LNXX.LD_DFR_BEG) as LD_DFR_BEG,
		max(LNXX.LD_DFR_END) as LD_DFR_END
	FROM	
		CDW..DFXX_BR_DFR_REQ FBXX
		INNER JOIN CDW..LNXX_BR_DFR_APV LNXX
			ON LNXX.BF_SSN = FBXX.BF_SSN
			AND LNXX.LF_DFR_CTL_NUM = FBXX.LF_DFR_CTL_NUM
	WHERE
		FBXX.LC_DFR_STA = 'A'
		AND LNXX.LC_STA_LONXX = 'A'
		AND FBXX.LC_STA_DFRXX = 'A'
		--AND FBXX.LC_FOR_TYP = 'XX'
		AND GETDATE() BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
	group by fbXX.bf_ssn,
	FBXX.LC_DFR_TYP ,
	LNXX.LN_SEQ
)DEF
	ON DEF.BF_SSN = FSXX.BF_SSN
	AND DEF.LN_SEQ = FSXX.LN_SEQ
LEFT JOIN CDW.CALC.RepaymentSchedules RS
	ON RS.BF_SSN = FSXX.BF_SSN
	AND RS.LN_SEQ = FSXX.LN_SEQ
	AND RS.CurrentGradation = X
LEFT JOIN CDW..LKXX_LS_CDE_LKP LKXXDW
	ON LKXXDW.PM_ATR = 'WC-DW-LON-STA'
	AND LKXXDW.PX_ATR_VAL = DWXX.WC_DW_LON_STA
LEFT JOIN CDW..LKXX_LS_CDE_LKP LKXXD
	ON LKXXD.PM_ATR = 'LC-DFR-TYP'
	AND LKXXD.PX_ATR_VAL = DEF.LC_DFR_TYP
LEFT JOIN CDW..LKXX_LS_CDE_LKP LKXXF
	ON LKXXF.PM_ATR = 'LC-FOR-TYP'
	AND LKXXF.PX_ATR_VAL = FORB.LC_FOR_TYP
ORDER BY
	CNH.[Borrower SSN]