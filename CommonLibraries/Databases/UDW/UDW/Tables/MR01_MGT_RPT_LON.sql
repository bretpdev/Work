﻿CREATE TABLE [dbo].[MR01_MGT_RPT_LON]
(
	[BF_SSN] VARCHAR(9) NOT NULL, 
	[LN_SEQ] SMALLINT NOT NULL, 
	[LA_CUR_PRI] DECIMAL(8,2) NULL, 
	[WA_ACR_BRI_RUN_DTE] DECIMAL(7,2) NULL, 
	[WD_RUN] DATETIME NULL, 
	[WN_DAY_DLQ_INT] DECIMAL(4,0) NULL, 
	[WD_DCO_INT] DATETIME NULL, 
	[WA_PSS_DU_INT] DECIMAL(7,2) NULL, 
	[WA_CUR_INT_DU] DECIMAL(7,2) NULL, 
	[WN_DAY_DLQ_ISL] DECIMAL(4,0) NULL, 
	[WD_DCO_ISL] DATETIME NULL, 
	[WA_PSS_DU_ISL] DECIMAL(7,2) NULL, 
	[WA_CUR_DU_ISL] DECIMAL(7,2) NULL, 
	[IC_LON_PGM] VARCHAR(6) NULL, 
	[WX_LON_PGM] VARCHAR(20) NULL, 
	[IF_BND_ISS] VARCHAR(8) NULL, 
	[LD_LON_1_DSB] DATETIME NULL, 
	[LD_LON_EFF_ADD] DATETIME NULL, 
	[LF_STU_SSN] VARCHAR(9) NULL, 
	[LD_STA_STU10] DATETIME NULL, 
	[LD_SCL_SPR] DATETIME NULL, 
	[LD_DFR_BEG] DATETIME NULL, 
	[LD_DFR_END] DATETIME NULL, 
	[LD_DFR_GRC_END] DATETIME NULL, 
	[LC_DFR_RSP] VARCHAR(3) NULL, 
	[WX_DFR_RSP] VARCHAR(20) NULL, 
	[LD_DFR_APL] DATETIME NULL, 
	[LD_FOR_BEG] DATETIME NULL, 
	[LD_FOR_END] DATETIME NULL, 
	[LC_FOR_RSP] VARCHAR(3) NULL, 
	[WX_FOR_RSP] VARCHAR(20) NULL, 
	[LD_FOR_APL] DATETIME NULL, 
	[WA_RPS_ISL_1] DECIMAL(7,2) NULL, 
	[WN_RPS_TRM_1] DECIMAL(3,0) NULL, 
	[WA_RPS_ISL_2] DECIMAL(7,2) NULL, 
	[WN_RPS_TRM_2] DECIMAL(3,0) NULL, 
	[WA_RPS_ISL_3] DECIMAL(7,2) NULL, 
	[WN_RPS_TRM_3] DECIMAL(3,0) NULL, 
	[WA_RPS_ISL_4] DECIMAL(7,2) NULL, 
	[WN_RPS_TRM_4] DECIMAL(3,0) NULL, 
	[WA_RPS_ISL_5] DECIMAL(7,2) NULL, 
	[WN_RPS_TRM_5] DECIMAL(3,0) NULL, 
	[WA_RPS_ISL_6] DECIMAL(7,2) NULL, 
	[WN_RPS_TRM_6] DECIMAL(3,0) NULL, 
	[WA_RPS_ISL_7] DECIMAL(7,2) NULL, 
	[WN_RPS_TRM_7] DECIMAL(3,0) NULL, 
	[LD_RPS_1_PAY_DU] DATETIME NULL, 
	[WC_ITR_TYP_1] VARCHAR(2) NULL, 
	[WX_ITR_TYP_1] VARCHAR(20) NULL, 
	[WR_ITR_1] DECIMAL(5,3) NULL, 
	[WD_ITR_EFF_BEG_1] DATETIME NULL, 
	[WC_ITR_TYP_2] VARCHAR(2) NULL, 
	[WX_ITR_TYP_2] VARCHAR(20) NULL, 
	[WR_ITR_2] DECIMAL(5,3) NULL, 
	[WD_ITR_EFF_BEG_2] DATETIME NULL, 
	[DM_PRS_1] VARCHAR(13) NULL, 
	[DM_PRS_MID] VARCHAR(13) NULL, 
	[DM_PRS_LST] VARCHAR(23) NULL, 
	[DM_PRS_LST_SFX] VARCHAR(4) NULL, 
	[DI_PHN_VLD] BIT NULL, 
	[DN_DOM_PHN_ARA] VARCHAR(3) NULL, 
	[IF_GTR] VARCHAR(6) NULL, 
	[LF_LON_OWN_CUR] VARCHAR(8) NULL, 
	[LF_DOE_SCL_ORG] VARCHAR(8) NULL, 
	[LF_DOE_SCL_ENR_CUR] VARCHAR(8) NULL, 
	[LF_GTR_RFR] VARCHAR(12) NULL, 
	[LD_END_GRC_PRD] DATETIME NULL, 
	[LC_ELG_SIN] BIT NULL, 
	[WX_ELG_SIN] VARCHAR(20) NULL, 
	[LF_CUR_POR] VARCHAR(20) NULL, 
	[LF_OWN_ORG_POR] VARCHAR(20) NULL, 
	[LC_LOC_PNT] VARCHAR(3) NULL, 
	[WX_LOC_PNT] VARCHAR(20) NULL, 
	[LD_OWN_EFF_SR] DATETIME NULL, 
	[WC_ISL_DLQ_CAT] CHAR(1) NULL, 
	[WX_ISL_DLQ_CAT] VARCHAR(20) NULL, 
	[WC_INT_DLQ_CAT] CHAR(1) NULL, 
	[WX_INT_DLQ_CAT] VARCHAR(20) NULL, 
	[WA_ORG_PRI] DECIMAL(8,2) NULL, 
	[WN_ATV_DSB] DECIMAL(3,0) NULL, 
	[WN_ACL_DSB] DECIMAL(3,0) NULL, 
	[WN_ANT_DSB] DECIMAL(3,0) NULL, 
	[WC_LON_STA] VARCHAR(2) NULL, 
	[WX_LON_STA] VARCHAR(10) NULL, 
	[WC_LON_SUB_STA] VARCHAR(2) NULL, 
	[WX_LON_SUB_STA] VARCHAR(12) NULL, 
	[WC_LON_CLM_STA] VARCHAR(2) NULL, 
	[WX_LON_CLM_STA] VARCHAR(20) NULL, 
	[WC_BR_PRS_STA] VARCHAR(2) NULL, 
	[WX_BR_PRS_STA] VARCHAR(20) NULL, 
	[LC_DFR_TYP] VARCHAR(2) NULL, 
	[WX_DFR_TYP] VARCHAR(20) NULL, 
	[LC_FOR_TYP] VARCHAR(2) NULL, 
	[WX_FOR_TYP] VARCHAR(20) NULL, 
	[LC_FOR_SUB_TYP] CHAR(1) NULL, 
	[WX_FOR_SUB_TYP] VARCHAR(20) NULL, 
	[LC_TYP_SCH_DIS] VARCHAR(2) NULL, 
	[WX_TYP_SCH_DIS] VARCHAR(20) NULL, 
	[LD_NTF_SCL_SPR] DATETIME NULL, 
	[LD_SPA_STP] DATETIME NULL, 
	[LD_SPA_STP_ENT] DATETIME NULL, 
	[LD_SPA_RTT] DATETIME NULL, 
	[LD_SPA_RTT_ENT] DATETIME NULL, 
	[WA_ACR_BRI_MTT] DECIMAL(7,2) NULL, 
	[WA_CUR_LTE_FEE] DECIMAL(6,2) NULL, 
	[WA_PRV_LTE_FEE] DECIMAL(6,2) NULL, 
	[LC_ST_BR_RSD_APL] VARCHAR(2) NULL, 
	[LC_STA_NEW_BR] VARCHAR(3) NULL, 
	[WF_NON_PR_ACT_REQ] VARCHAR(5) NULL, 
	[WD_FNL_DMD_BR] DATETIME NULL, 
	[WD_FNL_DMD_EDS] DATETIME NULL, 
	[LC_IND_BIL_SNT] CHAR(1) NULL, 
	[LC_BIL_MTD] CHAR(1) NULL, 
	[LD_DSB] DATETIME NULL, 
	[WA_LST_DSB_WK] DECIMAL(8,2) NULL, 
	[WI_LON_FUL_DSB_WK] BIT NULL, 

    PRIMARY KEY ([BF_SSN], 
				 [LN_SEQ]
				)
)
