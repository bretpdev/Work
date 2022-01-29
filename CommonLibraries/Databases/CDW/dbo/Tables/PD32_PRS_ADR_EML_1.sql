﻿CREATE TABLE [dbo].[PD32_PRS_ADR_EML] (
    [DF_PRS_ID]          CHAR (9)      NOT NULL,
    [DC_ADR_EML]         CHAR (1)      NOT NULL,
    [DF_CRT_DTS_PD32]    DATETIME      NOT NULL,
    [DC_STA_PD32]        CHAR (1)      NOT NULL,
    [DD_STA_PD32]        DATETIME      NULL,
    [DD_VER_ADR_EML]     DATETIME      NULL,
    [DD_CRT_PD32]        DATETIME      NULL,
    [DC_SRC_ADR]         VARCHAR (2)   NOT NULL,
    [DI_VLD_ADR_EML]     CHAR (1)      NOT NULL,
    [DF_LST_USR_PD32]    VARCHAR (8)   NOT NULL,
    [DF_LST_DTS_PD32]    DATETIME      NOT NULL,
    [DX_ADR_EML]         VARCHAR (254) NOT NULL,
    [DI_SND_LTR_EML]     CHAR (1)      NOT NULL,
    [DC_REA_EML_NOT_SND] CHAR (1)      NOT NULL,
    [DC_DL_COR_TYP]      CHAR (1)      NOT NULL,
    [DX_ADR_EML_UC]      VARCHAR (254) NOT NULL
);
