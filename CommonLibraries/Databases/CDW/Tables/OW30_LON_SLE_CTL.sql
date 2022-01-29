﻿CREATE TABLE [dbo].[OW30_LON_SLE_CTL] (
    [IF_LON_SLE]         VARCHAR (7)     NOT NULL,
    [IC_LON_SLE_STA]     CHAR (1)        NOT NULL,
    [IC_LON_SLE_TYP]     CHAR (1)        NOT NULL,
    [ID_LON_SLE]         DATE            NULL,
    [IF_SLL_OWN]         VARCHAR (8)     NOT NULL,
    [IM_SLL_CNC_1]       VARCHAR (13)    NOT NULL,
    [IM_SLL_CNC_LST]     VARCHAR (23)    NOT NULL,
    [IN_SLL_CNC_PHN_ARA] VARCHAR (3)     NOT NULL,
    [IN_SLL_CNC_PHN_XCH] VARCHAR (3)     NOT NULL,
    [IN_SLL_CNC_PHN_LCL] VARCHAR (4)     NOT NULL,
    [IN_SLL_CNC_PHN_XTN] VARCHAR (4)     NOT NULL,
    [IF_SLL_BND_ISS]     VARCHAR (8)     NOT NULL,
    [ID_SLL_LON_SLE_APV] DATE            NULL,
    [IF_BUY_OWN]         VARCHAR (8)     NULL,
    [IM_BUY_CNC_1]       VARCHAR (13)    NOT NULL,
    [IM_BUY_CNC_LST]     VARCHAR (23)    NOT NULL,
    [IN_BUY_CNC_PHN_ARA] VARCHAR (3)     NOT NULL,
    [IN_BUY_CNC_PHN_XCH] VARCHAR (3)     NOT NULL,
    [IN_BUY_CNC_PHN_LCL] VARCHAR (4)     NOT NULL,
    [IN_BUY_CNC_PHN_XTN] VARCHAR (4)     NOT NULL,
    [IF_BUY_BND_ISS]     VARCHAR (8)     NOT NULL,
    [ID_BUY_LON_SLE_APV] DATE            NULL,
    [IM_MKT_CNC_1]       VARCHAR (13)    NOT NULL,
    [IM_MKT_CNC_LST]     VARCHAR (23)    NOT NULL,
    [IN_MKT_CNC_PHN_ARA] VARCHAR (3)     NOT NULL,
    [IN_MKT_CNC_PHN_XCH] VARCHAR (3)     NOT NULL,
    [IN_MKT_CNC_PHN_LCL] VARCHAR (4)     NOT NULL,
    [IN_MKT_CNC_PHN_XTN] VARCHAR (4)     NOT NULL,
    [IM_TRF_CNC_1]       VARCHAR (13)    NOT NULL,
    [IM_TRF_CNC_LST]     VARCHAR (23)    NOT NULL,
    [IN_TRF_CNC_PHN_ARA] VARCHAR (3)     NOT NULL,
    [IN_TRF_CNC_PHN_XCH] VARCHAR (3)     NOT NULL,
    [IN_TRF_CNC_PHN_LCL] VARCHAR (4)     NOT NULL,
    [IN_TRF_CNC_PHN_XTN] VARCHAR (4)     NOT NULL,
    [IM_LEG_CNC_1]       VARCHAR (13)    NOT NULL,
    [IM_LEG_CNC_LST]     VARCHAR (23)    NOT NULL,
    [IN_LEG_CNC_PHN_ARA] VARCHAR (3)     NOT NULL,
    [IN_LEG_CNC_PHN_XCH] VARCHAR (3)     NOT NULL,
    [IN_LEG_CNC_PHN_LCL] VARCHAR (4)     NOT NULL,
    [IN_LEG_CNC_PHN_XTN] VARCHAR (4)     NOT NULL,
    [IC_FEE_ORG_RSB]     VARCHAR (1)     NOT NULL,
    [II_ACP_NEW_LON_SLE] VARCHAR (1)     NOT NULL,
    [IA_LON_TOT_MAX]     NUMERIC (11, 2) NOT NULL,
    [II_INT_ICL]         CHAR (1)        NOT NULL,
    [IN_LON_MAX]         NUMERIC (7)     NOT NULL,
    [II_SLE_LTR_TRG]     CHAR (1)        NOT NULL,
    [IF_SLE_LTR_SPC]     VARCHAR (10)    NOT NULL,
    [IF_LST_DTS_OW30]    DATETIME2 (7)   NOT NULL,
    [IF_BUY_POR]         CHAR (20)       NOT NULL,
    [IC_SLL_PNT_LOC]     VARCHAR (3)     NOT NULL,
    [IC_BUY_PNT_LOC]     VARCHAR (3)     NOT NULL,
    [IC_TIR_PCE_ASN]     CHAR (1)        NOT NULL,
    [II_LTE_FEE_WOF]     CHAR (1)        NOT NULL,
    [IC_LON_SLE_SEL_TYP] CHAR (1)        NOT NULL,
    [II_LTE_FEE_MAX_VAL] CHAR (1)        NOT NULL,
    [II_STP_SLE_LON_MAX] CHAR (1)        NOT NULL,
    [II_LEV_BR_LON_ELG]  CHAR (1)        NOT NULL,
    [IC_SEL_CRI_USR_APV] CHAR (1)        NOT NULL,
    [ID_SEL_CRI_USR_APV] DATE            NULL,
    [IF_SEL_CRI_USR_APV] VARCHAR (8)     NOT NULL,
    [ID_SEL_NXT_PLR]     DATE            NULL,
    [ID_LON_SLE_LST_PLR] DATE            NULL,
    [IT_SLE_LST_PLR]     TIME (0)        NULL,
    [IN_LON_SLE_BR_ELG]  INT             NULL,
    [IN_LON_SLE_LON_ELG] INT             NULL,
    [IA_CUR_PRI_ELG_LON] NUMERIC (12, 2) NULL,
    [IA_NSI_ELG_LON]     NUMERIC (11, 2) NULL,
    [IA_LTE_FEE_ELG_LON] NUMERIC (11, 2) NULL,
    [IN_IVL_SCH_NXT_SLE] SMALLINT        NULL,
    [IC_IVL_SCH_NXT_SLE] CHAR (1)        NOT NULL,
    [IN_IVL_SCH_NXT_PLR] SMALLINT        NULL,
    [IC_IVL_SCH_NXT_PLR] CHAR (1)        NOT NULL,
    [IX_TRG_FIL_SEL_CRI] VARCHAR (1024)  NOT NULL,
    [IC_RGN_RCV_DCV_LON] VARCHAR (2)     NOT NULL,
    [IX_DSC_BUY_OWN]     VARCHAR (144)   NOT NULL,
    [LA_BR_PRI_BAL_SLE]  NUMERIC (12, 2) NULL,
    [LC_BR_PRI_REL_SLE]  VARCHAR (2)     NOT NULL,
    [II_ORG_RGT_PUR_SLE] CHAR (1)        NOT NULL,
    [IF_GRP_SLE_KEY]     VARCHAR (8)     NULL,
    [IC_EFT_RIR_RSB]     CHAR (1)        NOT NULL,
    [IX_LON_SLE_CMT]     VARCHAR (70)    NOT NULL,
    [IC_BBS_RSB]         CHAR (1)        NOT NULL,
    [IC_LON_SLE_SUB_TYP] CHAR (1)        NOT NULL,
    [IA_SLE_LVL_TRF_FEE] NUMERIC (7, 2)  NULL,
    [IC_TRF_FEE_TYP]     CHAR (1)        NOT NULL,
    [IR_PRI_PER_FEE_RTE] NUMERIC (5, 3)  NULL,
    [II_ICL_CON_STP_PUR] CHAR (1)        NOT NULL,
    [ID_ECA_DCV_CRT_FIL] DATE            NULL,
    [II_ECA_PUT_DCV_APV] CHAR (1)        NOT NULL,
    [ID_ECA_CRT_BIL_SLE] DATE            NULL,
    [ID_CDU_REM_NTF]     DATE            NULL,
    [IC_FED_PGM_YR]      VARCHAR (3)     NOT NULL,
    [IF_FLS_DEA]         VARCHAR (5)     NOT NULL,
    [IF_DEA_IST_LIN_HLD] VARCHAR (8)     NOT NULL,
    [II_LON_SLE_ICL_IDT] CHAR (1)        NOT NULL,
    [II_ICL_CON_GRP_RLP] CHAR (1)        NOT NULL,
    [LI_PCV_OWN_EFF_DTE] CHAR (1)        NOT NULL,
    [II_PRE_SLE_LTR]     VARCHAR (1)     NOT NULL,
    [IC_DLA_CAN_LTR]     VARCHAR (2)     NOT NULL,
    [IC_PRE_SLE_LST_PRC] VARCHAR (2)     NOT NULL
);




