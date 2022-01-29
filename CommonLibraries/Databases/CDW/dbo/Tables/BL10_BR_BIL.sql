﻿CREATE TABLE [dbo].[BL10_BR_BIL] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [LD_BIL_CRT]         DATETIME        NOT NULL,
    [LN_SEQ_BIL_WI_DTE]  SMALLINT        NOT NULL,
    [LN_BIL_SRL]         NUMERIC (11)    NOT NULL,
    [LC_BIL_MTD]         CHAR (1)        NOT NULL,
    [LC_BIL_TYP]         CHAR (1)        NOT NULL,
    [LD_RBL_LST]         DATETIME        NULL,
    [LC_IND_BIL_SNT]     CHAR (1)        NOT NULL,
    [LD_BIL_DU]          DATETIME        NOT NULL,
    [LD_STA_BIL10]       DATETIME        NOT NULL,
    [LC_STA_BIL10]       CHAR (1)        NOT NULL,
    [LF_LST_DTS_BL10]    DATETIME        NOT NULL,
    [LC_EFT_EXT]         CHAR (1)        NOT NULL,
    [BN_EFT_SEQ]         SMALLINT        NULL,
    [LC_BR_BIL_SNT_REA]  CHAR (1)        NOT NULL,
    [LA_INT_PD_LST_STM]  NUMERIC (12, 2) NULL,
    [LA_FEE_PD_LST_STM]  NUMERIC (12, 2) NULL,
    [LA_PRI_PD_LST_STM]  NUMERIC (12, 2) NULL,
    [LD_LST_PAY_LST_STM] DATETIME        NULL,
    [LC_PCV_BIL]         CHAR (1)        NOT NULL
);

