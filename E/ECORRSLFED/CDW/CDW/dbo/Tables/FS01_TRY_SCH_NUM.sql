﻿CREATE TABLE [dbo].[FS01_TRY_SCH_NUM] (
    [PF_TRY_SCH_NUM]     VARCHAR (10)    NOT NULL,
    [PC_TRY_SCH_TYP]     VARCHAR (2)     NOT NULL,
    [PD_CRT_FS01]        DATE            NOT NULL,
    [PF_ORG_TRY_SCH_NUM] VARCHAR (10)    NULL,
    [PC_ORG_TRY_SCH_TYP] VARCHAR (2)     NULL,
    [PA_TOT_TRY_SCH_AMT] NUMERIC (14, 2) NULL,
    [PN_TOT_TRY_SCH_ITM] NUMERIC (7)     NULL,
    [PD_TRY_SCH_DPS]     DATE            NULL,
    [PF_LST_USR_FS01]    VARCHAR (8)     NOT NULL,
    [PF_LST_DTS_FS01]    DATETIME2 (7)   NOT NULL,
    [PA_FED_RCC_TOT]     NUMERIC (14, 2) NULL,
    [PD_FED_RCC_DPS]     DATE            NULL,
    [PN_FED_RCC_TOT_ITM] NUMERIC (7)     NULL,
    CONSTRAINT [PK_FS01_TRY_SCH_NUM] PRIMARY KEY CLUSTERED ([PF_TRY_SCH_NUM] ASC, [PC_TRY_SCH_TYP] ASC) WITH (FILLFACTOR = 95)
);
