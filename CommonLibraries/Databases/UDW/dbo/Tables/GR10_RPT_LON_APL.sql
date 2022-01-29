﻿CREATE TABLE [dbo].[GR10_RPT_LON_APL]
(
	BF_SSN CHAR(9) NOT NULL,
	WN_SEQ_GR10 SMALLINT NOT NULL,
	LN_SEQ SMALLINT NULL,
	AN_SEQ SMALLINT NULL,
	WC_STA_GR10 CHAR(1) NOT NULL,
	WF_BR_SSN_OLD CHAR(9) NOT NULL,
	LF_STU_SSN CHAR(9) NOT NULL,
	WF_STU_SSN_OLD CHAR(9) NOT NULL,
	WF_GTR_RFR CHAR(16) NOT NULL,
	WD_LON_GTR DATE NULL,
	IC_LON_PGM CHAR(6) NOT NULL,
	LD_LON_1_DSB DATE NULL,
	LF_LON_CUR_OWN CHAR(8) NOT NULL,
	IF_GTR CHAR(6) NOT NULL,
	WD_TRM_BEG DATE NULL,
	WD_TRM_END DATE NULL,
	WF_LON_ALT CHAR(17) NOT NULL,
	WN_LON_ALT_SEQ SMALLINT NULL,
	LC_REA_SCL_SPR CHAR(2) NOT NULL,
	LD_SCL_SPR DATE NULL,
	LF_DOE_SCL_ENR_CUR CHAR(8) NOT NULL,
	LC_SCR_SCL_SPR CHAR(2) NOT NULL,
	WC_ACA_GDE_LEV CHAR(2) NOT NULL,
	LD_SCL_CER_STU_STA DATE NULL,
	LC_CAM_LON_STA CHAR(2) NOT NULL,
	LD_DFR_FOR_END DATE NULL,
	LC_DFR_FOR_TYP CHAR(2) NOT NULL,
	LF_CAM_DFR_SCL_ENR CHAR(8) NOT NULL,
	WI_DFR_PIO_1_PAY CHAR(1) NOT NULL,
	LD_RPD_SR DATE NULL,
	WI_REC_03_RPT CHAR(1) NOT NULL,
	WD_REC_03_RPT DATE NULL,
	WI_REC_04_RPT CHAR(1) NOT NULL,
	WD_REC_04_RPT DATE NULL,
	WI_REC_05_RPT CHAR(1) NOT NULL,
	WD_REC_05_RPT DATE NULL,
	WI_REC_07_RPT CHAR(1) NOT NULL,
	WD_REC_07_RPT DATE NULL,
	WI_REC_15_RPT CHAR(1) NOT NULL,
	WD_REC_15_RPT DATE NULL,
	WI_REC_03S_RPT CHAR(1) NOT NULL,
	WD_REC_03S_RPT DATE NULL,
	WI_REC_05S_RPT CHAR(1) NOT NULL,
	WD_REC_05S_RPT DATE NULL,
	WI_REC_13_RPT CHAR(1) NOT NULL,
	WD_REC_13_RPT DATE NULL,
	WD_REC_13_ACK_BY DATE NULL,
	WI_REC_13_ACK CHAR(1) NOT NULL,
	WI_REC_14_RPT CHAR(1) NOT NULL,
	WD_REC_14_RPT DATE NULL,
	WD_REC_14_ACK_BY DATE NULL,
	WI_REC_14_ACK CHAR(1) NOT NULL,
	WI_REC_24_RPT CHAR(1) NOT NULL,
	WD_REC_24_RPT DATE NULL,
	WD_REC_24_ACK_BY DATE NULL,
	WI_REC_24_ACK CHAR(1) NOT NULL,
	LD_PIF_RPT DATE NULL,
	WF_PPR_OWN_BRH CHAR(4) NOT NULL,
	LA_LON_AMT_GTR DECIMAL(8,2) NOT NULL,
	WD_ORG_RPD DATE NULL,
	WD_SR_CUR_RPD DATE NULL,
	AA_REQ_SCL DECIMAL(6,0) NOT NULL,
	WA_EDU_CST DECIMAL(5,0) NOT NULL,
	WA_EFC DECIMAL(5,0) NOT NULL,
	WA_EDU_AID_TOT DECIMAL(5,0) NOT NULL,
	LD_DFR_FOR_BEG DATE NULL,
	LC_LON_EXT_ORG_SRC CHAR(1) NOT NULL,
	LF_ORG_RGN CHAR(8) NOT NULL,
	WF_SER_CDE_CAM CHAR(6) NOT NULL,
	LD_ENR_STA_EFF_CAM DATE NULL,
	WF_GTR_PAR_CAM_RPT CHAR(8) NOT NULL,
	WF_LST_DTS_GR10 TIMESTAMP NOT NULL,
	WA_LON_AMT_GTR_NEW DECIMAL(8,2) NOT NULL,
	WC_ADR_TYP_CHG CHAR(1) NOT NULL,
	WI_REC_03_RPT_NME CHAR(1) NOT NULL,
	WI_REC_03_RPT_DOB CHAR(1) NOT NULL,
	WI_REC_03S_RPT_NME CHAR(1) NOT NULL,
	WI_REC_03S_RPT_DOB CHAR(1) NOT NULL,
	WC_ADR_CHG_TYP_STU CHAR(1) NOT NULL,
	WD_SCL_SPR_PRV_RPT DATE NULL,
	LD_OWN_EFF_SR DATE NULL,
	WD_CAM_SER_RSB DATE NULL,
	WF_CAM_PRV_OWN CHAR(8) NOT NULL,
	WF_CAM_PRV_SER CHAR(6) NOT NULL,
	LA_CUR_PRI DECIMAL(8,2) NOT NULL,
	WD_CUR_PRI DATE NULL,
	WA_TOT_BRI_OTS DECIMAL(8,2) NOT NULL,
	WD_CLC_THU DATE NULL,
	LC_ITR_TYP CHAR(2) NOT NULL,
	WI_REC_16_RPT CHAR(1) NOT NULL,
	WD_REC_16_RPT DATE NULL,
	WD_REC_16_ACK_BY DATE NULL,
	WI_REC_16_ACK CHAR(1) NOT NULL,
	WI_REC_16_RPT_SER CHAR(1) NOT NULL,
	WI_REC_16_RPT_LDR CHAR(1) NOT NULL,
	WI_REC_19_RPT CHAR(1) NOT NULL,
	WD_REC_19_RPT DATE NULL,
	WI_REC_26_RPT CHAR(1) NOT NULL,
	WD_REC_26_RPT DATE NULL,
	WI_REC_19S_RPT CHAR(1) NOT NULL,
	WD_REC_19S_RPT DATE NULL,
	LF_RGL_CAT_LP06 CHAR(7) NOT NULL,
	WI_CAM_LON_FUL_DSB CHAR(1) NOT NULL,
	WC_CAM_OWN_RPT_MTD CHAR(1) NOT NULL,
	AC_GTR_BG_APV CHAR(1) NOT NULL,
	WC_MPN_TYP CHAR(1) NOT NULL,
	WC_MPN_REV_REA CHAR(2) NOT NULL,
	WD_MPN_EXP DATE NULL,
	WI_REC_27_RPT CHAR(1) NOT NULL,
	WD_REC_27_RPT DATE NULL,
	WD_BR_SIG_MPN DATE NULL,
	WF_ORG_LDR CHAR(8) NOT NULL,
	WD_MPN_ORG_RGT_SLD DATE NULL,
	WF_PPR_OWN_BRH_PRV CHAR(4) NOT NULL,
	WF_PPR_OWN_BRH_ORG CHAR(4) NOT NULL,
	WI_RPT_BR_HME_PHN CHAR(1) NOT NULL,
	WI_RPT_BR_WRK_PHN CHAR(1) NOT NULL,
	WI_RPT_BR_OTH_PHN CHAR(1) NOT NULL,
	WI_RPT_STU_HME_PHN CHAR(1) NOT NULL,
	WI_RPT_STU_WRK_PHN CHAR(1) NOT NULL,
	WI_RPT_STU_OTH_PHN CHAR(1) NOT NULL,
	WC_ECA_PTC CHAR(2) NOT NULL,
	WI_ECA_FUL_FUD CHAR(1) NOT NULL,
	WF_BR_SSN_ACP_NDS CHAR(9) NOT NULL,
	WF_STU_SSN_ACP_NDS CHAR(9) NOT NULL,
	WD_LON_GTR_OLD DATE NULL,
	WD_LON_GTR_ACP DATE NULL,
	WC_SEP_LON_OLD CHAR(1) NOT NULL,
	WC_SEP_LON_NEW CHAR(1) NOT NULL,
	WC_SEP_LON_ACP CHAR(1) NOT NULL,
	WF_DOE_SCL_ORG_OLD CHAR(8) NOT NULL,
	WF_DOE_SCL_ORG_NEW CHAR(8) NOT NULL,
	WF_DOE_SCL_ORG_ACP CHAR(8) NOT NULL,
	WC_NDS_RPT_STA CHAR(1) NOT NULL,
	WD_NDS_RPT_STA DATE NULL,
	WC_NDS_SBM_ACT CHAR(1) NOT NULL,
	WD_NDS_SBM_ACT DATE NULL,
	WD_NDS_ITL_RPT DATE NULL,
	WI_USE_RPY_RPT_OVR CHAR(1) NOT NULL,
	LF_PRV_GTR CHAR(6) NOT NULL,
	WD_LST_RPT_NDS DATE NULL,
	WD_LST_ACP_NDS DATE NULL,
	WR_LN_INT DECIMAL(7,3) NULL,
	WN_CUM_MTH_DFR SMALLINT NULL,
	WN_CUM_MTH_FOR SMALLINT NULL,
	WD_LST_LN_PAY DATE NULL,
	WA_CUM_FEE_PD_2DT DECIMAL(10,0) NULL,
	WA_CUM_INT_PD_2DT DECIMAL(10,0) NULL,
	WA_CUM_PRI_PD_2DT DECIMAL(10,0) NULL,
	WC_NDS_PRS_RPT CHAR(1) NOT NULL,
	WD_NDS_PRS_RPT DATE NULL,
	WC_NDS_ADR_RPT CHAR(1) NOT NULL,
	WD_NDS_ADR_RPT DATE NULL,
	WC_NDS_EML_RPT CHAR(1) NOT NULL,
	WD_NDS_EML_RPT DATE NULL,
	WC_NDS_LN_REC_RPT CHAR(1) NOT NULL,
	WD_NDS_LN_REC_RPT DATE NULL,
	WC_NDS_LN_SUP_RPT CHAR(1) NOT NULL,
	WD_NDS_LN_SUP_RPT DATE NULL,
	WC_NDS_LN_STA_RPT CHAR(1) NOT NULL,
	WD_NDS_LN_STA_RPT DATE NULL,
	WC_NDS_LN_DSB_RPT CHAR(1) NOT NULL,
	WD_NDS_LN_DSB_RPT DATE NULL,
	WC_NDS_LN_CAN_RPT CHAR(1) NOT NULL,
	WD_NDS_LN_CAN_RPT DATE NULL,
	WC_NDS_LN_REB_RPT CHAR(1) NOT NULL,
	WD_NDS_LN_REB_RPT DATE NULL,
	WC_NDS_INT_BNF_RPT CHAR(1) NOT NULL,
	WD_NDS_INT_BNF_RPT DATE NULL,
	WC_NDS_RPD_TRM_RPT CHAR(1) NOT NULL,
	WD_NDS_RPD_TRM_RPT DATE NULL,
	WC_NDS_LN_FGV_RPT CHAR(1) NOT NULL,
	WD_NDS_LN_FGV_RPT DATE NULL,
	WC_NDS_LN_DCH_RPT CHAR(1) NOT NULL,
	WD_NDS_LN_DCH_RPT DATE NULL,
	WC_NDS_LN_RFD_RPT CHAR(1) NOT NULL,
	WD_NDS_LN_RFD_RPT DATE NULL,
	WC_NDS_OTS_BAL_RPT CHAR(1) NOT NULL,
	WD_NDS_OTS_BAL_RPT DATE NULL,
	WC_NDS_LN_CUR_ST CHAR(1) NOT NULL,
	WD_NDS_LN_CUR_ST DATE NULL,
	WC_NDS_RPD_SPS_RPT CHAR(1) NOT NULL,
	WD_NDS_RPD_SPS_RPT DATE NULL,
	WC_NDS_SCL_ENR_RPT CHAR(1) NOT NULL,
	WD_NDS_SCL_ENR_RPT DATE NULL,
	WC_NDS_SCL_TRF_RPT CHAR(1) NOT NULL,
	WD_NDS_SCL_TRF_RPT DATE NULL,
	WC_NDS_MPN_REC_RPT CHAR(1) NOT NULL,
	WD_NDS_MPN_REC_RPT DATE NULL,
	WC_NDS_CMP_HST_RPT CHAR(1) NOT NULL,
	WD_NDS_CMP_HST_RPT DATE NULL,
	WC_NDS_DEL_INU_RPT CHAR(1) NOT NULL,
	WD_NDS_DEL_INU_RPT DATE NULL,
	WA_OTS_FEE_BAL DECIMAL(10,0) NULL,
	WD_LST_CAP_INT DATE NULL,
	WA_CUM_CAP_INT DECIMAL(10,0) NULL,
	WC_NDS_PRS_RPT_STU CHAR(1) NOT NULL,
	WD_NDS_PRS_RPT_STU DATE NULL,
	WC_NDS_ADR_RPT_STU CHAR(1) NOT NULL,
	WD_NDS_ADR_RPT_STU DATE NULL,
	WC_NDS_EML_RPT_STU CHAR(1) NOT NULL,
	WD_NDS_EML_RPT_STU DATE NULL,
	LC_WOF_WUP_REA CHAR(1) NOT NULL,
	WC_NDS_PHN_RPT CHAR(1) NOT NULL,
	WD_NDS_PHN_RPT DATE NULL,
	WC_NDS_PHN_RPT_STU CHAR(1) NOT NULL,
	WD_NDS_PHN_RPT_STU DATE NULL,
	WI_RPT_BR_MBL_PHN CHAR(1) NOT NULL,
	WI_RPT_STU_MBL_PHN CHAR(1) NOT NULL,
	LD_DLQ_OCC DATE NULL,
	WC_NDS_PRS_RPT_END CHAR(1) NOT NULL,
	WD_NDS_PRS_RPT_END DATE NULL,
	WC_NDS_ADR_RPT_END CHAR(1) NOT NULL,
	WD_NDS_ADR_RPT_END DATE NULL,
	WC_NDS_EML_RPT_END CHAR(1) NOT NULL,
	WD_NDS_EML_RPT_END DATE NULL,
	WC_NDS_PHN_RPT_END CHAR(1) NOT NULL,
	WD_NDS_PHN_RPT_END DATE NULL,
	WI_RPT_END_HME_PHN CHAR(1) NOT NULL,
	WI_RPT_END_WRK_PHN CHAR(1) NOT NULL,
	WI_RPT_END_OTH_PHN CHAR(1) NOT NULL,
	WI_RPT_END_MBL_PHN CHAR(1) NOT NULL,
	LF_EDS CHAR(9) NOT NULL,
	WF_EDS_OLD CHAR(9) NOT NULL,
	LI_BR_GRP_RLP CHAR(1) NOT NULL,
	LF_LON_GRP_WI_BR SMALLINT NULL,
	WI_RTS_NDS_ITL_RPT CHAR(1) NOT NULL,
	WI_PSV_LFG_PTC CHAR(1) NOT NULL,
	WC_NDS_SPC_CNS_RPT CHAR(1) NOT NULL,
	WD_NDS_SPC_CNS_RPT DATE NULL,
	WC_NDS_PST_TRF_RPT CHAR(1) NOT NULL,
	WD_NDS_PST_TRF_RPT DATE NULL,
	WC_NDS_TEA_GRT_RPT CHAR(1) NOT NULL,
	WD_NDS_TEA_GRT_RPT DATE NULL,
	WD_TEA_GRT_LST_UPD DATE NULL,
	WC_NDS_PSV_PAY_RPT CHAR(1) NOT NULL,
	WD_NDS_PSV_PAY_RPT DATE NULL,
	WC_NDS_UND_LON_RPT CHAR(1) NOT NULL,
	WD_NDS_UND_LON_RPT DATE NULL,
	WC_NDS_SUL_RPT CHAR(1) NOT NULL,
	WD_NDS_SUL_RPT DATE NULL,
	LD_LON_STA_SR_RPT DATE NULL,
	LD_LON_SUS_SR_RPT DATE NULL,
	LI_RCL_LON_STA_HST CHAR(1) NOT NULL,
	WC_NDS_SCRA_RPT CHAR(1) NOT NULL,
	WD_NDS_SCRA_RPT DATE NULL,
	WC_NDS_PAY_REC_RPT CHAR(1) NOT NULL,
	WD_NDS_PAY_REC_RPT DATE NULL,
	WD_ERL_PAY_RPT_NDS DATE NULL,
	WC_NDS_PSV_EMP_RPT CHAR(1) NOT NULL,
	WD_NDS_PSV_EMP_RPT DATE NULL,
	WD_LST_ISL_BIL DATE NULL,
	WA_LST_ISL_BIL DECIMAL(12,2)
	CONSTRAINT [PKGR10_RPT_LON_APL] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [WN_SEQ_GR10] ASC)
)
