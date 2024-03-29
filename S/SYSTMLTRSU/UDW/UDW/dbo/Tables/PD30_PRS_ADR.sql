﻿CREATE TABLE [dbo].[PD30_PRS_ADR] (
    [DF_PRS_ID]       VARCHAR (9)   NOT NULL,
    [DC_ADR]          CHAR (1)      NOT NULL,
    [DD_STA_PDEM30]   DATE          NULL,
    [DD_VER_ADR]      DATE          NULL,
    [DI_VLD_ADR]      CHAR (1)      NOT NULL,
    [DF_ZIP_CDE]      VARCHAR (17)  NOT NULL,
    [DM_CT]           VARCHAR (20)  NOT NULL,
    [DX_STR_ADR_3]    VARCHAR (30)  NOT NULL,
    [DX_STR_ADR_2]    VARCHAR (30)  NOT NULL,
    [DX_STR_ADR_1]    VARCHAR (30)  NOT NULL,
    [DC_DOM_ST]       VARCHAR (2)   NOT NULL,
    [DF_LST_USR_PD30] VARCHAR (8)   NOT NULL,
    [DF_3PT_ADR]      VARCHAR (9)   NOT NULL,
    [DC_3PT_ADR]      VARCHAR (2)   NOT NULL,
    [DC_SRC_ADR]      VARCHAR (2)   NOT NULL,
    [DM_FGN_CNY]      VARCHAR (25)  NOT NULL,
    [DM_FGN_ST]       VARCHAR (15)  NOT NULL,
    [DF_LST_DTS_PD30] DATETIME2 (7) NULL,
    [DX_DLV_PTR_BCD]  VARCHAR (14)  NULL,
    [DC_FGN_CNY]      VARCHAR (2)   NOT NULL,
    [DD_DSB_ADR_BEG]  DATE          NULL,
    [DD_DSB_ADR_END]  DATE          NULL,
    CONSTRAINT [PK_PD30_PRS_ADR1] PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [DC_ADR] ASC) WITH (FILLFACTOR = 95)
);

