﻿CREATE TABLE [dbo].[GRS4_RPY_PLN_DAT] (
    [DF_PRS_ID]          VARCHAR (9)     NOT NULL,
    [LF_NDS_LON_RSP_LAB] VARCHAR (17)    NOT NULL,
    [LD_NDS_LIS_REQ]     DATE            NOT NULL,
    [LN_RPY_PLN_DAT_SEQ] SMALLINT        NOT NULL,
    [LF_CRT_DTS_GRS4]    DATETIME2 (7)   NOT NULL,
    [LF_CRT_USR_GRS4]    VARCHAR (12)    NOT NULL,
    [LD_RPY_PLN_DIS]     DATE            NULL,
    [LD_SCH_PAY_EFF]     DATE            NULL,
    [LC_RPY_PLN_TYP]     VARCHAR (2)     NOT NULL,
    [LN_RPY_PLN_TRM]     NUMERIC (3)     NULL,
    [LA_SCH_RPY_PLN_PAY] NUMERIC (12, 2) NULL,
    [LN_DAY_MTH_BIL_DU]  NUMERIC (2)     NULL,
    [LC_LON_BIL_TYP]     VARCHAR (2)     NOT NULL,
    [LD_1_PAY_DU]        DATE            NULL,
    [LI_RPY_PLN_NAM]     CHAR (1)        NOT NULL,
    [LD_RPY_PLN_ESM_POF] DATE            NULL,
    [LA_STD_STD_SCH_PAY] NUMERIC (12, 2) NULL,
    [LA_PMN_STD_SCH_PAY] NUMERIC (12, 2) NULL,
    [LC_NPA_FIN_HDS_REA] CHAR (1)        NOT NULL,
    [LD_IDR_ANV]         DATE            NULL,
    [LA_RPY_TOT_CTH_UP]  NUMERIC (12, 2) NULL,
    [LA_RPY_SCH_CTH_UP]  NUMERIC (12, 2) NULL,
    CONSTRAINT [PK_GRS4] PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [LF_NDS_LON_RSP_LAB] ASC, [LD_NDS_LIS_REQ] ASC, [LN_RPY_PLN_DAT_SEQ] ASC)
);
