USE CDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.LN80_LON_BIL_CRF
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.LN80_LON_BIL_CRF
--		'
--	) 

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(LN80.LD_LST_DTS_LN80), '1-1-1900 00:00:00'), 21) FROM LN80_LON_BIL_CRF LN80)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN80_LON_BIL_CRF LN80
	USING
		(
			SELECT
				*
			FROM
				OPENQUERY
				(
					LEGEND,
					''
						SELECT
							LN80.*
						FROM
							PKUB.LN80_LON_BIL_CRF LN80
						-- comment WHERE clause for full table refresh
						WHERE
							LN80.LD_LST_DTS_LN80 > ''''' + @LastRefresh + '''''
					''
				) 
		) L ON L.BF_SSN = LN80.BF_SSN AND L.LN_SEQ = LN80.LN_SEQ AND L.LD_BIL_CRT = LN80.LD_BIL_CRT AND L.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE AND L.LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			LN80.LD_BIL_CRT = L.LD_BIL_CRT,
			LN80.LN_SEQ_BIL_WI_DTE = L.LN_SEQ_BIL_WI_DTE,
			LN80.LN_BIL_OCC_SEQ = L.LN_BIL_OCC_SEQ,
			LN80.LA_BIL_CUR_DU = L.LA_BIL_CUR_DU,
			LN80.LA_BIL_PAS_DU = L.LA_BIL_PAS_DU,
			LN80.LC_STA_LON80 = L.LC_STA_LON80,
			LN80.LA_NSI_BIL = L.LA_NSI_BIL,
			LN80.LA_CUR_PRN_BIL = L.LA_CUR_PRN_BIL,
			LN80.LC_LON_STA_BIL = L.LC_LON_STA_BIL,
			LN80.LR_INT_BIL = L.LR_INT_BIL,
			LN80.LD_LST_DTS_LN80 = L.LD_LST_DTS_LN80,
			LN80.LI_PSB = L.LI_PSB,
			LN80.LD_BIL_DU_LON = L.LD_BIL_DU_LON,
			LN80.LC_BIL_TYP_LON = L.LC_BIL_TYP_LON,
			LN80.LI_FNL_BIL_LON = L.LI_FNL_BIL_LON,
			LN80.LD_STA_LON80 = L.LD_STA_LON80,
			LN80.LA_BIL_DU_PRT = L.LA_BIL_DU_PRT,
			LN80.LA_TOT_BIL_STS = L.LA_TOT_BIL_STS,
			LN80.LA_PCV_BIL_STS = L.LA_PCV_BIL_STS,
			LN80.LF_DFR_CTL_NUM = L.LF_DFR_CTL_NUM,
			LN80.LF_FOR_CTL_NUM = L.LF_FOR_CTL_NUM,
			LN80.LD_LTE_FEE_ASS = L.LD_LTE_FEE_ASS,
			LN80.LA_LTE_FEE_ASS = L.LA_LTE_FEE_ASS,
			LN80.LI_LTE_FEE_OVR = L.LI_LTE_FEE_OVR,
			LN80.LC_LTE_FEE_WAV_REA = L.LC_LTE_FEE_WAV_REA,
			LN80.LD_BIL_STS_RIR_TOL = L.LD_BIL_STS_RIR_TOL,
			LN80.LI_BIL_DLQ_OVR_RIR = L.LI_BIL_DLQ_OVR_RIR,
			LN80.LF_USR_DLQ_OVR_RIR = L.LF_USR_DLQ_OVR_RIR,
			LN80.LA_LTE_FEE_OTS_PRT = L.LA_LTE_FEE_OTS_PRT,
			LN80.LD_LTE_FEE_WAV = L.LD_LTE_FEE_WAV,
			LN80.LD_RP_RTE_2B_DTR = L.LD_RP_RTE_2B_DTR,
			LN80.LC_RPY_OPT_AWD_BIL = L.LC_RPY_OPT_AWD_BIL,
			LN80.LN_CU_SEQ = L.LN_CU_SEQ,
			LN80.LC_RPD_RLF_ETR = L.LC_RPD_RLF_ETR,
			LN80.LA_LTE_FEE_EST_BIL = L.LA_LTE_FEE_EST_BIL,
			LN80.LN_DAY_FEE_GRC_BIL = L.LN_DAY_FEE_GRC_BIL,
			LN80.LA_INT_PD_2DT_BIL = L.LA_INT_PD_2DT_BIL,
			LN80.LA_FEE_PD_2DT_BIL = L.LA_FEE_PD_2DT_BIL,
			LN80.LA_PRI_PD_2DT_BIL = L.LA_PRI_PD_2DT_BIL,
			LN80.LI_PSV_FGV_BIL_OVR = L.LI_PSV_FGV_BIL_OVR,
			LN80.LA_PIO_BIL_PFH = L.LA_PIO_BIL_PFH,
			LN80.LA_RPYE_CTH_ISL = L.LA_RPYE_CTH_ISL,
			LN80.LA_RPYE_CTH_STS = L.LA_RPYE_CTH_STS,
			LN80.LA_TOT_NSB_INT_OTS = L.LA_TOT_NSB_INT_OTS,
			LN80.LD_NXT_PAY_DUE_AHD = L.LD_NXT_PAY_DUE_AHD,
			LN80.LA_NXT_TOT_DUE_AHD = L.LA_NXT_TOT_DUE_AHD
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ,
			LD_BIL_CRT,
			LN_SEQ_BIL_WI_DTE,
			LN_BIL_OCC_SEQ,
			LA_BIL_CUR_DU,
			LA_BIL_PAS_DU,
			LC_STA_LON80,
			LA_NSI_BIL,
			LA_CUR_PRN_BIL,
			LC_LON_STA_BIL,
			LR_INT_BIL,
			LD_LST_DTS_LN80,
			LI_PSB,
			LD_BIL_DU_LON,
			LC_BIL_TYP_LON,
			LI_FNL_BIL_LON,
			LD_STA_LON80,
			LA_BIL_DU_PRT,
			LA_TOT_BIL_STS,
			LA_PCV_BIL_STS,
			LF_DFR_CTL_NUM,
			LF_FOR_CTL_NUM,
			LD_LTE_FEE_ASS,
			LA_LTE_FEE_ASS,
			LI_LTE_FEE_OVR,
			LC_LTE_FEE_WAV_REA,
			LD_BIL_STS_RIR_TOL,
			LI_BIL_DLQ_OVR_RIR,
			LF_USR_DLQ_OVR_RIR,
			LA_LTE_FEE_OTS_PRT,
			LD_LTE_FEE_WAV,
			LD_RP_RTE_2B_DTR,
			LC_RPY_OPT_AWD_BIL,
			LN_CU_SEQ,
			LC_RPD_RLF_ETR,
			LA_LTE_FEE_EST_BIL,
			LN_DAY_FEE_GRC_BIL,
			LA_INT_PD_2DT_BIL,
			LA_FEE_PD_2DT_BIL,
			LA_PRI_PD_2DT_BIL,
			LI_PSV_FGV_BIL_OVR,
			LA_PIO_BIL_PFH,
			LA_RPYE_CTH_ISL,
			LA_RPYE_CTH_STS,
			LA_TOT_NSB_INT_OTS,
			LD_NXT_PAY_DUE_AHD,
			LA_NXT_TOT_DUE_AHD
		)
		VALUES 
		(
			L.BF_SSN,
			L.LN_SEQ,
			L.LD_BIL_CRT,
			L.LN_SEQ_BIL_WI_DTE,
			L.LN_BIL_OCC_SEQ,
			L.LA_BIL_CUR_DU,
			L.LA_BIL_PAS_DU,
			L.LC_STA_LON80,
			L.LA_NSI_BIL,
			L.LA_CUR_PRN_BIL,
			L.LC_LON_STA_BIL,
			L.LR_INT_BIL,
			L.LD_LST_DTS_LN80,
			L.LI_PSB,
			L.LD_BIL_DU_LON,
			L.LC_BIL_TYP_LON,
			L.LI_FNL_BIL_LON,
			L.LD_STA_LON80,
			L.LA_BIL_DU_PRT,
			L.LA_TOT_BIL_STS,
			L.LA_PCV_BIL_STS,
			L.LF_DFR_CTL_NUM,
			L.LF_FOR_CTL_NUM,
			L.LD_LTE_FEE_ASS,
			L.LA_LTE_FEE_ASS,
			L.LI_LTE_FEE_OVR,
			L.LC_LTE_FEE_WAV_REA,
			L.LD_BIL_STS_RIR_TOL,
			L.LI_BIL_DLQ_OVR_RIR,
			L.LF_USR_DLQ_OVR_RIR,
			L.LA_LTE_FEE_OTS_PRT,
			L.LD_LTE_FEE_WAV,
			L.LD_RP_RTE_2B_DTR,
			L.LC_RPY_OPT_AWD_BIL,
			L.LN_CU_SEQ,
			L.LC_RPD_RLF_ETR,
			L.LA_LTE_FEE_EST_BIL,
			L.LN_DAY_FEE_GRC_BIL,
			L.LA_INT_PD_2DT_BIL,
			L.LA_FEE_PD_2DT_BIL,
			L.LA_PRI_PD_2DT_BIL,
			L.LI_PSV_FGV_BIL_OVR,
			L.LA_PIO_BIL_PFH,
			L.LA_RPYE_CTH_ISL,
			L.LA_RPYE_CTH_STS,
			L.LA_TOT_NSB_INT_OTS,
			L.LD_NXT_PAY_DUE_AHD,
			L.LA_NXT_TOT_DUE_AHD
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	--    DELETE
	;
'

PRINT @SQLStatement
EXEC (@SQLStatement)