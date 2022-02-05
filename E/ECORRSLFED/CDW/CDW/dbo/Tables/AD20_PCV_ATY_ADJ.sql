﻿CREATE TABLE [dbo].[AD20_PCV_ATY_ADJ] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [LD_FAT_ADJ_REQ]     DATE            NOT NULL,
    [LN_SEQ_FAT_ADJ_REQ] SMALLINT        NOT NULL,
    [LA_FAT_ADJ_LTE_FEE] NUMERIC (7, 2)  NULL,
    [LA_FAT_ADJ_PCL_FEE] NUMERIC (7, 2)  NULL,
    [LD_FAT_ADJ_REQ_BEG] DATE            NOT NULL,
    [LD_NEW_FAT_ADJ_REQ] DATE            NULL,
    [LA_FAT_ADJ_ILG_PRI] NUMERIC (8, 2)  NULL,
    [LA_FAT_ADJ_CUR_PRI] NUMERIC (8, 2)  NULL,
    [LA_FAT_ADJ_NSI]     NUMERIC (7, 2)  NULL,
    [LF_USR_FAT_ADJ_REQ] CHAR (8)        NOT NULL,
    [LC_TYP_FAT_ADJ_REQ] CHAR (2)        NOT NULL,
    [LD_STA_FAT_ADJ_REQ] DATE            NOT NULL,
    [LC_STA_FAT_ADJ_REQ] CHAR (1)        NOT NULL,
    [LX_CMT_FAT_ADJ_REQ] VARCHAR (360)   NOT NULL,
    [LF_LST_DTS_AD20]    DATETIME2 (7)   NOT NULL,
    [LD_RMT_BCH_INI_FAT] DATE            NULL,
    [LC_RMT_BCH_IPT_FAT] CHAR (1)        NULL,
    [LN_RMT_BCH_SEQ_FAT] SMALLINT        NULL,
    [LN_RMT_SEQ_FAT]     NUMERIC (6)     NULL,
    [LN_RMT_ITM_FAT]     NUMERIC (3)     NULL,
    [LN_RMT_ITM_SEQ_FAT] SMALLINT        NULL,
    [LN_FAT_SEQ]         SMALLINT        NULL,
    [LN_SEQ_PCV_ADJ_REQ] SMALLINT        NULL,
    [LN_SPA_SEQ]         SMALLINT        NULL,
    [LN_SEQ]             SMALLINT        NULL,
    [LF_STU_SSN]         CHAR (9)        NULL,
    [LN_STU_SPR_SEQ]     SMALLINT        NULL,
    [LC_FAT_REV_REA]     CHAR (1)        NOT NULL,
    [LA_PCV_ADJ_SIN_APL] NUMERIC (7, 2)  NULL,
    [LA_CVN_R78_INT_PD]  NUMERIC (7, 2)  NULL,
    [LA_CVN_R78_INT_MAX] NUMERIC (7, 2)  NULL,
    [IF_LON_SLE]         CHAR (7)        NULL,
    [PC_FAT_TYP]         CHAR (2)        NULL,
    [PC_FAT_SUB_TYP]     CHAR (2)        NULL,
    [LC_ATT_INA_PD_AHD]  CHAR (1)        NOT NULL,
    [LN_BR_DSB_SEQ]      SMALLINT        NULL,
    [LN_LON_BBS_SEQ]     SMALLINT        NULL,
    [LN_LON_BBT_SEQ]     SMALLINT        NULL,
    [LA_FAT_ADJ_MSC_FEE] NUMERIC (12, 2) NULL,
    [LA_FAT_DL_ADJ_REB]  NUMERIC (12, 2) NULL,
    [LC_PRC_ADJ_SPS]     CHAR (3)        NOT NULL,
    [LI_PRC_ADJ_SPS_OVR] CHAR (1)        NOT NULL,
    [LD_DSB_ADJ_ACP_COD] DATE            NULL,
    [LF_WOF_WUP_PRS]     CHAR (9)        NOT NULL,
    [LC_WOF_WUP_PRS_TYP] CHAR (1)        NOT NULL,
    [LC_WOF_WUP_REA]     CHAR (1)        NOT NULL,
    [LF_LST_SRC_AD20]    CHAR (16)       NULL,
    [LC_ADJ_ACP_MTD]     CHAR (2)        NOT NULL,
    [LC_REV_REA]         CHAR (1)        NOT NULL,
    [LC_CHO_REA]         CHAR (1)        NOT NULL,
    CONSTRAINT [PK_AD20_PCV_ATY_ADJ] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LD_FAT_ADJ_REQ] ASC, [LN_SEQ_FAT_ADJ_REQ] ASC) WITH (FILLFACTOR = 95)
);
