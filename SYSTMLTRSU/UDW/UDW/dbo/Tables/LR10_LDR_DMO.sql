﻿CREATE TABLE [dbo].[LR10_LDR_DMO] (
    [IF_DOE_LDR]         VARCHAR (8)   NOT NULL,
    [IM_LDR_SHO]         VARCHAR (20)  NOT NULL,
    [IM_LDR_FUL]         VARCHAR (40)  NOT NULL,
    [IF_ACC_EFT_LDR]     VARCHAR (17)  NOT NULL,
    [IF_ABA_EFT_LDR]     VARCHAR (10)  NOT NULL,
    [IC_LDR_TYP]         VARCHAR (2)   NOT NULL,
    [II_FUD_ORG_FEE]     CHAR (1)      NOT NULL,
    [II_FUD_INS_PRM]     CHAR (1)      NOT NULL,
    [IF_LST_USR_LR10]    VARCHAR (8)   NOT NULL,
    [IF_LST_DTS_LR10]    DATETIME2 (7) NOT NULL,
    [IC_LDR_MRG_TYP]     CHAR (1)      NOT NULL,
    [IF_LDR_MRG_TO]      VARCHAR (8)   NULL,
    [II_LDR_BR_MPN_CNF]  CHAR (1)      NOT NULL,
    [II_LDR_SFD_MPN_CNF] CHAR (1)      NOT NULL,
    [II_LDR_USF_MPN_CNF] CHAR (1)      NOT NULL,
    [IF_LDR_MPN_CNF_LTR] VARCHAR (10)  NOT NULL,
    [IF_LDR_MPN_NTF_LTR] VARCHAR (10)  NOT NULL,
    [II_LDR_KST_SEL]     CHAR (1)      NOT NULL,
    [II_LN10_MPN_LKP]    CHAR (1)      NOT NULL,
    [IF_LDR_SER_BUR_PTC] VARCHAR (8)   NOT NULL,
    [II_RFL_LDR]         CHAR (1)      NOT NULL,
    [ID_LDR_MRG_EFF]     DATE          NULL,
    [II_ALW_PSB_MPN]     CHAR (1)      NOT NULL,
    [II_LDR_MLT_DSB_NTF] CHAR (1)      NOT NULL,
    [II_COL_SER_DOC_FRM] CHAR (1)      NOT NULL,
    [II_CRD_UNN_MEM_VER] CHAR (1)      NOT NULL,
    [ID_LDR_ORG_END]     DATE          NULL,
    [IC_LDR_FUD_RTN_MTD] CHAR (1)      NOT NULL,
    CONSTRAINT [PK_LR10_LDR_DMO] PRIMARY KEY CLUSTERED ([IF_DOE_LDR] ASC)
);

