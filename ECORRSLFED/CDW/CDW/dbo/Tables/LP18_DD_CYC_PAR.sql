﻿CREATE TABLE [dbo].[LP18_DD_CYC_PAR] (
    [PC_STA_LPD18]       CHAR (1)       NOT NULL,
    [IC_LON_PGM]         CHAR (6)       NOT NULL,
    [PF_RGL_CAT]         CHAR (7)       NOT NULL,
    [IF_GTR]             CHAR (6)       NOT NULL,
    [PD_EFF_SR_LPD18]    DATE           NOT NULL,
    [PD_STA_LPD18]       DATE           NOT NULL,
    [PN_SER_DAY_SKP_RAC] NUMERIC (3)    NOT NULL,
    [PC_CLM_SBM_LEV]     CHAR (1)       NOT NULL,
    [PN_DAY_ISL_CLM_SBM] NUMERIC (3)    NOT NULL,
    [PN_DAY_RTN_CLM_SBM] NUMERIC (3)    NOT NULL,
    [PN_DAY_PCL_CAN_DLQ] NUMERIC (3)    NOT NULL,
    [PN_DAY_TRF_GRC]     NUMERIC (3)    NOT NULL,
    [PN_DAY_DTH_CLM_SBM] NUMERIC (3)    NOT NULL,
    [PN_DAY_DSA_CLM_SBM] NUMERIC (3)    NOT NULL,
    [PN_DAY_BKR_CLM_SBM] NUMERIC (3)    NOT NULL,
    [PN_DAY_INT_CLM_SBM] NUMERIC (3)    NOT NULL,
    [PN_DAY_CLM_CAN_DLQ] NUMERIC (3)    NOT NULL,
    [PF_LTR_CAN_CLM]     CHAR (10)      NOT NULL,
    [PF_REQ_ACT_CAN_CLM] CHAR (5)       NULL,
    [PF_LST_USR_LP18]    CHAR (8)       NOT NULL,
    [PF_TSK_CLM_CAN]     CHAR (5)       NULL,
    [PN_DAY_ACR_CLM_SBM] NUMERIC (3)    NOT NULL,
    [PC_PCA_FMT_PGM]     CHAR (8)       NOT NULL,
    [PI_MNL_PCA]         CHAR (1)       NOT NULL,
    [PI_APV_REQ_DSA_CLM] CHAR (1)       NOT NULL,
    [PN_DAY_REJ_CLM_SBM] NUMERIC (3)    NOT NULL,
    [PN_DAY_CLM_PAY_SBM] NUMERIC (3)    NOT NULL,
    [PC_CLM_CAN]         CHAR (1)       NOT NULL,
    [PN_CU_RDI_CYC]      NUMERIC (3)    NOT NULL,
    [PN_ISL_CU_PAY]      NUMERIC (2)    NOT NULL,
    [PF_CU_RNS_FRM]      CHAR (10)      NOT NULL,
    [PD_EFF_END_LPD18]   DATE           NULL,
    [PF_LST_DTS_LP18]    DATETIME2 (7)  NOT NULL,
    [PF_REQ_ACT_BKR_CLM] CHAR (5)       NOT NULL,
    [PF_REQ_ACT_DSA_CLM] CHAR (5)       NOT NULL,
    [PF_REQ_ACT_DTH_CLM] CHAR (5)       NOT NULL,
    [PN_DAX_SBM_CLM_ABC] NUMERIC (3)    NOT NULL,
    [PN_MDY_SBM_CLM_ABC] NUMERIC (3)    NOT NULL,
    [PI_RTT_ABC_IVL_ADR] CHAR (1)       NOT NULL,
    [PI_CU_OLY_WI_RDI]   CHAR (1)       NOT NULL,
    [PF_REQ_ACT_48L]     CHAR (5)       NULL,
    [PF_BCH_LPD_CHG]     CHAR (8)       NOT NULL,
    [PF_REQ_ACT_RSI_CLM] CHAR (5)       NULL,
    [PF_REQ_ACT_DLQ_CLM] CHAR (5)       NULL,
    [PF_REQ_ACT_CU_CLM]  CHAR (5)       NULL,
    [PI_CU_ISL]          CHAR (1)       NOT NULL,
    [PI_CU_INT]          CHAR (1)       NOT NULL,
    [PC_VLD_DTH_CER]     CHAR (1)       NOT NULL,
    [PF_DSA_FRM]         CHAR (10)      NOT NULL,
    [PF_FRM_CLM_CHK_LIS] CHAR (10)      NOT NULL,
    [PI_INT_ACR_RTN_CLM] CHAR (1)       NOT NULL,
    [PA_ISL_PAY_TOL]     NUMERIC (5, 2) NOT NULL,
    [PA_INT_PAY_TOL]     NUMERIC (5, 2) NOT NULL,
    [PI_MAX_RPD_XCL_CU]  CHAR (1)       NOT NULL,
    [PN_SR_PCL_GTR_UPD]  NUMERIC (3)    NULL,
    [PN_STP_PCL_GTR_UPD] NUMERIC (3)    NULL,
    [PI_GTR_UPD_CSB]     CHAR (1)       NOT NULL,
    [PI_PUR_IN_CLM]      CHAR (1)       NOT NULL,
    [PC_GTR_INF_CLM]     CHAR (1)       NOT NULL,
    [PF_CCI_RCP]         CHAR (8)       NOT NULL,
    [PC_CCI_CLM_LON_ID]  CHAR (1)       NOT NULL,
    [PC_CLM_RVW_STA]     CHAR (1)       NOT NULL,
    [PC_CLM_FMT_PGM]     CHAR (8)       NOT NULL,
    [PC_CCI_CLM_SRT_CRI] CHAR (1)       NOT NULL,
    [PF_CCI_ITR_CLM_TSK] CHAR (5)       NOT NULL,
    [PF_CCI_ISL_CLM_TSK] CHAR (5)       NOT NULL,
    [PF_CCI_CER_CLM_TSK] CHAR (5)       NOT NULL,
    [PF_CCI_DTH_CLM_TSK] CHAR (5)       NOT NULL,
    [PF_CCI_BKR_CLM_TSK] CHAR (5)       NOT NULL,
    [PF_CCI_DSA_CLM_TSK] CHAR (5)       NOT NULL,
    [PF_CCI_CU_CLM_TSK]  CHAR (5)       NOT NULL,
    [PF_CCI_SCL_CLM_TSK] CHAR (5)       NOT NULL,
    [PF_CCI_ILG_CLM_TSK] CHAR (5)       NOT NULL,
    [PC_CCI_ET_LON_ID]   CHAR (1)       NOT NULL,
    [PI_GTR_ACP_TYP_55]  CHAR (1)       NOT NULL,
    [PR_ISL_BIL_TOL]     NUMERIC (5, 3) NULL,
    [PR_INT_BIL_TOL]     NUMERIC (5, 3) NULL,
    [PI_XPS_CLM_ALW]     CHAR (1)       NOT NULL,
    [PN_SBM_CLM_BKR_ADS] NUMERIC (3)    NULL,
    [PN_SBM_RTN_REJ_ADS] NUMERIC (3)    NULL,
    [PF_TSK_CLM_BKR_ADS] CHAR (5)       NOT NULL,
    CONSTRAINT [PK_LP18_DD_CYC_PAR] PRIMARY KEY CLUSTERED ([PC_STA_LPD18] ASC, [IC_LON_PGM] ASC, [PF_RGL_CAT] ASC, [IF_GTR] ASC, [PD_EFF_SR_LPD18] ASC)
);

