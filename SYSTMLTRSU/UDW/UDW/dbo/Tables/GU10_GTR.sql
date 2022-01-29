﻿CREATE TABLE [dbo].[GU10_GTR] (
    [IF_GTR]             VARCHAR (6)  NOT NULL,
    [IF_ACC_EFT_GTR]     VARCHAR (17) NULL,
    [IF_ABA_EFT_GTR]     VARCHAR (9)  NULL,
    [IM_GTR_SHO]         VARCHAR (20) NULL,
    [IM_GTR_FUL]         VARCHAR (40) NULL,
    [II_RPT_GTR_SCL]     BIT          NULL,
    [II_RPT_GTR_LDR]     BIT          NULL,
    [II_ASN_RFR_NUM]     BIT          NULL,
    [IC_RPT_DSB_TYP]     CHAR (1)     NULL,
    [II_GTR_NAT_PTC]     CHAR (1)     NULL,
    [IF_LST_USR_GU10]    VARCHAR (8)  NULL,
    [IF_LST_DTS_GU10]    DATETIME     NULL,
    [IF_GTR_PRN]         VARCHAR (6)  NULL,
    [IC_GTR_TYP]         CHAR (1)     NULL,
    [II_GTR_PRE_DSB_M]   CHAR (1)     NULL,
    [II_GTR_NOG_REQ]     CHAR (1)     NULL,
    [II_GTR_CNL_PTC]     CHAR (1)     NULL,
    [II_GTR_MNF_DSB_RSN] CHAR (1)     NULL,
    [IC_GTR_FEE_RPT_MTH] CHAR (1)     NULL,
    [II_RAL_SCY_OVR]     CHAR (1)     NULL,
    [II_GTR_ALW_PSB_MPN] CHAR (1)     NULL,
    [II_GTR_CHS_PTC]     CHAR (1)     NULL,
    [II_GTR_VER_FEE_RQR] CHAR (1)     NULL,
    [II_GTR_WHL_DOL_DSB] CHAR (1)     NULL,
    PRIMARY KEY CLUSTERED ([IF_GTR] ASC)
);

