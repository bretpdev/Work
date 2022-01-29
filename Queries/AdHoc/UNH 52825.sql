
SELECT DISTINCT 
	Duster.DF_SPE_ACC_ID,
	Duster.LN_SEQ,
	Duster.LF_LON_ALT,
	Duster.LD_DLQ_OCC,
	Duster.LD_AUX_STA_UPD,
	Duster.LD_SBM_CLM_PCL,
	Duster.LC_TYP_SCH_DIS,
	Duster.LC_AUX_STA,
	Duster.LA_PAY_XPC,
	Duster.LD_CRT_LON66,
	Duster.DAYS_DELQ,
	Duster.RE_DEFAULT,
	Duster.Payment,
	Duster.IBR,
	Duster.LD_PIF_RPT
FROM OPENQUERY(
DUSTER,
'
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID,
		LN10.LN_SEQ,
		LN10.LF_LON_ALT,
		LN16.LD_DLQ_OCC,
		DC01.LD_AUX_STA_UPD,
		LN40.LD_SBM_CLM_PCL,
		Repayment.LC_TYP_SCH_DIS,
		DC01.LC_AUX_STA,
		DC01.LA_PAY_XPC,
		Repayment.LD_CRT_LON66,
		DAYS(CURRENT DATE) - DAYS((COALESCE(LN16.LD_DLQ_OCC,CURRENT DATE))) AS DAYS_DELQ,
		CASE WHEN LN40.BF_SSN IS NULL THEN ''N'' ELSE ''Y'' END AS RE_DEFAULT,
		CASE WHEN Repayment.LC_TYP_SCH_DIS IN (''IB'',''IL'') THEN ''Y'' ELSE ''N'' END AS IBR,
		LN10.LD_PIF_RPT,
		LN66.LA_RPS_ISL AS Payment
	FROM
		OLWHRM1.PD10_PRS_NME PD10
		INNER JOIN OLWHRM1.LN10_LON LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN OLWHRM1.DC01_LON_CLM_INF DC01
			ON DC01.BF_SSN = LN10.BF_SSN
			AND DC01.AF_APL_ID = LN10.LF_LON_ALT
			AND DC01.LC_AUX_STA = ''10'' --Rehabbed
			AND DC01.LA_PAY_XPC = 5.00
		LEFT OUTER JOIN OLWHRM1.LN90_FIN_ATY LN90
			ON LN90.BF_SSN = LN10.BF_SSN
			AND LN90.LN_SEQ = LN10.LN_SEQ
			AND LN90.PC_FAT_TYP = ''10''
			AND LN90.PC_FAT_SUB_TYP = ''30''
			AND LN90.LC_STA_LON90 = ''A''
		LEFT OUTER JOIN OLWHRM1.LN40_LON_CLM_PCL LN40
			ON LN40.BF_SSN = LN10.BF_SSN
			AND LN40.LN_SEQ = LN40.LN_SEQ
			AND LN40.LC_TYP_REC_CLP_LON IN (''1'',''4'',''6'') --Active claims that arent preclaim
			AND COALESCE(DC01.LD_AUX_STA_UPD,''3000-01-01'') < COALESCE(LN40.LD_SBM_CLM_PCL,''1900-01-01'') 
		LEFT OUTER JOIN OLWHRM1.LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = LN10.BF_SSN
			AND LN16.LN_SEQ = LN10.LN_SEQ
			AND LN16.LC_STA_LON16 = ''1''
			AND LN16.LD_DLQ_MAX = CURRENT DATE - 1 DAYS --currently delinquent
		LEFT OUTER JOIN 
		(
			SELECT
				LN65.BF_SSN,
				LN65.LN_SEQ,
				LN65.LC_TYP_SCH_DIS,
				MIN(LN66.LD_CRT_LON66) AS LD_CRT_LON66
			FROM
				OLWHRM1.LN65_LON_RPS LN65
				INNER JOIN OLWHRM1.LN66_LON_RPS_SPF LN66
					ON LN65.BF_SSN = LN66.BF_SSN
					AND LN65.LN_SEQ = LN66.LN_SEQ
					AND LN65.LN_RPS_SEQ = LN66.LN_RPS_SEQ
			WHERE
				LN66.LN_GRD_RPS_SEQ = 1
				--AND LN65.LC_STA_LON65 = ''A''
			GROUP BY
				LN65.BF_SSN,
				LN65.LN_SEQ,
				LN65.LC_TYP_SCH_DIS
		) Repayment
			ON Repayment.BF_SSN = LN10.BF_SSN
			AND Repayment.LN_SEQ = LN10.LN_SEQ
			AND COALESCE(Repayment.LD_CRT_LON66,''1900-01-01'') > COALESCE(DC01.LD_AUX_STA_UPD,''1900-01-01'')
		LEFT OUTER JOIN OLWHRM1.LN66_LON_RPS_SPF LN66
			ON LN66.BF_SSN = Repayment.BF_SSN
			AND LN66.LN_SEQ = Repayment.LN_SEQ
			AND LN66.LD_CRT_LON66 = Repayment.LD_CRT_LON66
			AND LN66.LN_GRD_RPS_SEQ = 1
	WHERE
		LN90.LN_SEQ IS NULL
	ORDER BY
		DAYS_DELQ		
'
)Duster 
ORDER BY
	Duster.DF_SPE_ACC_ID,
	Duster.LN_SEQ