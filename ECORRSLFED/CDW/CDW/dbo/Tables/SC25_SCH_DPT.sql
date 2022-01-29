﻿CREATE TABLE [dbo].[SC25_SCH_DPT] (
    [IF_DOE_SCL]         VARCHAR (8)   NOT NULL,
    [IC_SCL_DPT]         VARCHAR (3)   NOT NULL,
    [IC_SCL_DPT_CRT_IDM] VARCHAR (2)   NOT NULL,
    [IC_SCL_DOM_ST]      VARCHAR (2)   NOT NULL,
    [ID_SCL_VER_DPT]     DATE          NULL,
    [IF_SCL_ZIP_CDE]     VARCHAR (9)   NOT NULL,
    [II_SCL_VLD_ADR]     CHAR (1)      NOT NULL,
    [IM_SCL_CNC_1]       VARCHAR (10)  NOT NULL,
    [IM_SCL_CNC_LST]     VARCHAR (20)  NOT NULL,
    [IM_SCL_CT]          VARCHAR (20)  NOT NULL,
    [IN_SCL_DOM_FAX_ARA] VARCHAR (3)   NOT NULL,
    [IN_SCL_DOM_FAX_XCH] VARCHAR (3)   NOT NULL,
    [IN_SCL_DOM_FAX_LCL] VARCHAR (4)   NOT NULL,
    [IN_SCL_DOM_PHN_ARA] VARCHAR (3)   NOT NULL,
    [IN_SCL_DOM_PHN_XCH] VARCHAR (3)   NOT NULL,
    [IN_SCL_DOM_PHN_LCL] VARCHAR (4)   NOT NULL,
    [IN_SCL_DOM_PHN_XTN] VARCHAR (4)   NOT NULL,
    [IX_SCL_CNC_TTL]     VARCHAR (20)  NOT NULL,
    [IX_SCL_STR_ADR_1]   VARCHAR (30)  NOT NULL,
    [IX_SCL_STR_ADR_2]   VARCHAR (30)  NOT NULL,
    [IX_SCL_STR_ADR_3]   VARCHAR (30)  NOT NULL,
    [IM_SCL_FGN_CNY]     VARCHAR (15)  NOT NULL,
    [IM_SCL_FGN_ST]      VARCHAR (15)  NOT NULL,
    [IN_SCL_FGN_FAX_INL] VARCHAR (3)   NOT NULL,
    [IN_SCL_FGN_FAX_CNY] VARCHAR (3)   NOT NULL,
    [IN_SCL_FGN_FAX_CT]  VARCHAR (4)   NOT NULL,
    [IN_SCL_FGN_FAX_LCL] VARCHAR (7)   NOT NULL,
    [IN_SCL_FGN_PHN_INL] VARCHAR (3)   NOT NULL,
    [IN_SCL_FGN_PHN_CNY] VARCHAR (3)   NOT NULL,
    [IN_SCL_FGN_PHN_CT]  VARCHAR (4)   NOT NULL,
    [IN_SCL_FGN_PHN_LCL] VARCHAR (7)   NOT NULL,
    [IN_SCL_FGN_PHN_XTN] VARCHAR (4)   NOT NULL,
    [IF_LST_DTS_SC25]    DATETIME2 (7) NOT NULL,
    [IM_SCL_DPT_FUL]     VARCHAR (40)  NOT NULL,
    [IM_SCL_DPT_SHO]     VARCHAR (20)  NOT NULL,
    CONSTRAINT [PK_SC25_SCH_DPT] PRIMARY KEY CLUSTERED ([IF_DOE_SCL] ASC, [IC_SCL_DPT] ASC)
);

