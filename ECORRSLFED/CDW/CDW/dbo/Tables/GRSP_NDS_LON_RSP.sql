﻿CREATE TABLE [dbo].[GRSP_NDS_LON_RSP] (
    [DF_PRS_ID]          CHAR (9)        NOT NULL,
    [LF_NDS_LON_RSP_LAB] CHAR (17)       NOT NULL,
    [LD_NDS_LIS_REQ]     DATE            NOT NULL,
    [LF_CRT_DTS_GRSP]    DATETIME2 (7)   NOT NULL,
    [LF_CRT_USR_GRSP]    VARCHAR (12)    NOT NULL,
    [LF_CRT_SRC_GRSP]    VARCHAR (16)    NOT NULL,
    [LF_FED_AWD]         VARCHAR (18)    NOT NULL,
    [LN_FED_AWD_SEQ]     NUMERIC (3)     NULL,
    [IC_LON_PGM]         VARCHAR (6)     NOT NULL,
    [LF_FED_CLC_RSK]     VARCHAR (6)     NOT NULL,
    [WC_ACA_GDE_LEV]     VARCHAR (2)     NOT NULL,
    [WF_ORG_LDR]         VARCHAR (8)     NOT NULL,
    [LF_ORG_SER]         VARCHAR (8)     NOT NULL,
    [LF_CUR_LDR]         VARCHAR (8)     NOT NULL,
    [LF_CUR_SER]         VARCHAR (8)     NOT NULL,
    [LF_CUR_GTR]         VARCHAR (6)     NOT NULL,
    [LD_NDS_LIS_PIF]     DATE            NULL,
    [LF_NDS_LIS_PIF_SRC] VARCHAR (8)     NOT NULL,
    [LC_NDS_LIS_LON_STA] VARCHAR (2)     NOT NULL,
    [LD_NDS_LIS_LON_STA] DATE            NULL,
    [LA_ORG_LON_BAL]     NUMERIC (12, 2) NOT NULL,
    [LA_TOT_DSB]         NUMERIC (12, 2) NOT NULL,
    [LA_CUR_PRI]         NUMERIC (8, 2)  NOT NULL,
    [WA_TOT_BRI_OTS]     NUMERIC (8, 2)  NOT NULL,
    [LD_RPD_SR]          DATE            NULL,
    [LA_LON_BAL_RPY]     NUMERIC (12, 2) NOT NULL,
    [LR_ITR]             NUMERIC (5, 3)  NOT NULL,
    [LR_STT_ITR]         NUMERIC (5, 3)  NOT NULL,
    [LI_UDL_PLS_LON]     CHAR (1)        NOT NULL,
    [BI_SUB_USE_LMT_APL] CHAR (1)        NOT NULL,
    [BD_SUB_USE_LMT_APL] DATE            NULL,
    [LI_LON_JNT_CON]     CHAR (1)        NOT NULL,
    [LF_LON_CON_APL_AWD] VARCHAR (21)    NOT NULL,
    [LC_LON_INT_RTE_TYP] CHAR (1)        NOT NULL,
    [LF_DOE_SCL_ORG]     VARCHAR (8)     NOT NULL,
    [IdrIneligible]      BIT             NULL,
    [IcrIneligible]      BIT             NULL,
    CONSTRAINT [PK_GRSP_NDS_LON_RSP] PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [LF_NDS_LON_RSP_LAB] ASC, [LD_NDS_LIS_REQ] ASC)
);



