﻿CREATE TABLE [dbo].[DF10_BR_DFR_REQ] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [LF_DFR_CTL_NUM]     VARCHAR (3)     NOT NULL,
    [LC_DFR_TYP]         VARCHAR (2)     NOT NULL,
    [LI_REQ_FOR_DLQ]     CHAR (1)        NOT NULL,
    [LI_DFR_BR_SIG]      CHAR (1)        NOT NULL,
    [LI_DFR_CER_SIG]     CHAR (1)        NOT NULL,
    [LI_DFR_LCO_SIG]     CHAR (1)        NOT NULL,
    [LD_DFR_REQ_BEG]     DATE            NOT NULL,
    [LD_DFR_REQ_END]     DATE            NOT NULL,
    [LC_DFR_REQ_COR]     VARCHAR (3)     NOT NULL,
    [LF_USR_CRT_REQ_DFR] VARCHAR (8)     NOT NULL,
    [LD_CRT_REQ_DFR]     DATE            NOT NULL,
    [LI_CAP_DFR_INT_REQ] CHAR (1)        NOT NULL,
    [LC_DFR_STA]         CHAR (1)        NOT NULL,
    [LI_DFR_NSI_PAY]     CHAR (1)        NOT NULL,
    [LD_STA_DFR10]       DATE            NOT NULL,
    [LC_STA_DFR10]       CHAR (1)        NOT NULL,
    [LF_DOE_SCL_DFR]     VARCHAR (8)     NOT NULL,
    [LI_ACD_CCL]         CHAR (1)        NOT NULL,
    [LI_FUL_ACT_DTY]     CHAR (1)        NOT NULL,
    [LI_RES_NG]          CHAR (1)        NOT NULL,
    [LC_ENR_STA]         CHAR (1)        NOT NULL,
    [LI_NTF_STM]         CHAR (1)        NOT NULL,
    [LI_LON_RCV_ENR_PRD] CHAR (1)        NOT NULL,
    [LI_EMP_AGY_50_MLE]  CHAR (1)        NOT NULL,
    [LI_RQR_ATT_LIS]     CHAR (1)        NOT NULL,
    [LC_ARA_TSH]         VARCHAR (3)     NOT NULL,
    [LD_NXT_TRM_BEG]     DATE            NULL,
    [LD_RGR_EMP_AGY]     DATE            NULL,
    [LD_UMP_BEG]         DATE            NULL,
    [LI_ALI_FOR_REQ]     CHAR (1)        NOT NULL,
    [LF_LST_DTS_DF10]    DATETIME2 (7)   NOT NULL,
    [LF_DFR_PRV]         VARCHAR (3)     NULL,
    [LF_STU_SSN]         VARCHAR (9)     NULL,
    [LD_NTF_DFR_END]     DATE            NULL,
    [LI_BR_EMP_FT]       CHAR (1)        NOT NULL,
    [LI_BR_SBM_POO_INC]  CHAR (1)        NOT NULL,
    [LI_BR_SBM_FED_IXR]  CHAR (1)        NOT NULL,
    [LI_BR_SBM_CER_HWG]  CHAR (1)        NOT NULL,
    [LI_BR_MIN_INC_RQR]  CHAR (1)        NOT NULL,
    [LI_BR_SBM_POO_SLP]  CHAR (1)        NOT NULL,
    [LI_BR_SBM_WRT_STM]  CHAR (1)        NOT NULL,
    [LI_BR_SBM_CER_APC]  CHAR (1)        NOT NULL,
    [LI_BR_ENR_LST_6MO]  CHAR (1)        NOT NULL,
    [LI_BR_SBM_CER_CHB]  CHAR (1)        NOT NULL,
    [LI_BR_SBM_MIL_ID]   CHAR (1)        NOT NULL,
    [LI_BR_SBM_MIL_ORD]  CHAR (1)        NOT NULL,
    [LI_MIN_TME_DSA]     CHAR (1)        NOT NULL,
    [LI_RGR_EMP_AGY]     CHAR (1)        NOT NULL,
    [LD_DFR_CER]         DATE            NULL,
    [LD_DFR_INF_CER]     DATE            NULL,
    [LI_BR_POO_UMP_BNF]  CHAR (1)        NOT NULL,
    [LI_CMK_ELG_DFR]     CHAR (1)        NOT NULL,
    [LC_DFR_SUB_TYP]     VARCHAR (2)     NOT NULL,
    [LI_DFR_DOC_PVD]     CHAR (1)        NOT NULL,
    [LI_DFR_RQR_CMP]     CHAR (1)        NOT NULL,
    [LD_BR_REQ_DFR_BEG]  DATE            NULL,
    [LD_DFR_SPT_DOC_BEG] DATE            NULL,
    [LD_DFR_SPT_DOC_END] DATE            NULL,
    [LI_DFR_SPT_DOC_ACP] CHAR (1)        NOT NULL,
    [LC_DFR_DNL_USR_ENT] VARCHAR (3)     NOT NULL,
    [LI_DFR_DOC_SPT_REQ] CHAR (1)        NOT NULL,
    [LI_REQ_PST_DFR_DFR] CHAR (1)        NOT NULL,
    [LI_REQ_IN_SCL_DFR]  CHAR (1)        NOT NULL,
    [LD_STP_ENR_MIN_HT]  DATE            NULL,
    [LA_BR_PAY_CHK_JOB]  NUMERIC (12, 2) NULL,
    [LC_BR_PAY_CHK_FRQ]  VARCHAR (2)     NOT NULL,
    [LC_BR_EMP_STA]      CHAR (1)        NOT NULL,
    [LN_BR_FAM_SIZ]      NUMERIC (3)     NULL,
    [LC_FED_POV_GID_ST]  VARCHAR (2)     NOT NULL,
    [LA_MTH_FED_MIN_WGE] NUMERIC (12, 2) NULL,
    [LA_BR_CLC_POV]      NUMERIC (12, 2) NULL,
    [LC_SEL_EHD_DFR_TYP] CHAR (1)        NOT NULL
);
