﻿CREATE TABLE [dbo].[PD42_PRS_PHN] (
    [DF_PRS_ID]          CHAR (9)      NOT NULL,
    [DC_PHN]             CHAR (1)      NOT NULL,
    [DT_PHN_BST_CL]      TIME (0)      NULL,
    [DD_PHN_VER]         DATE          NULL,
    [DI_PHN_VLD]         CHAR (1)      NOT NULL,
    [DN_PHN_XTN]         VARCHAR (5)   NOT NULL,
    [DN_DOM_PHN_LCL]     VARCHAR (4)   NOT NULL,
    [DN_DOM_PHN_XCH]     VARCHAR (3)   NOT NULL,
    [DN_DOM_PHN_ARA]     VARCHAR (3)   NOT NULL,
    [DI_PHN_WTS]         CHAR (1)      NOT NULL,
    [DX_PHN_TME_ZNE]     VARCHAR (5)   NOT NULL,
    [DF_LST_USR_PD42]    VARCHAR (8)   NOT NULL,
    [DN_FGN_PHN_INL]     VARCHAR (3)   NOT NULL,
    [DN_FGN_PHN_CNY]     VARCHAR (3)   NOT NULL,
    [DN_FGN_PHN_CT]      VARCHAR (5)   NOT NULL,
    [DN_FGN_PHN_LCL]     VARCHAR (11)  NOT NULL,
    [DF_LST_DTS_PD42]    DATETIME2 (7) NOT NULL,
    [DD_CRT_PD42]        DATE          NULL,
    [DC_NO_HME_PHN]      CHAR (1)      NOT NULL,
    [DD_NO_HME_PHN]      DATE          NULL,
    [DC_PHN_SRC]         VARCHAR (2)   NOT NULL,
    [DD_ORG_DB_CRT_PD40] DATE          NOT NULL,
    [DI_HST_OLY_PD40]    CHAR (1)      NOT NULL,
    [DC_ALW_ADL_PHN]     CHAR (1)      NOT NULL
);

