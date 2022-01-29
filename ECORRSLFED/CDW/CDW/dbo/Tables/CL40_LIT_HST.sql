﻿CREATE TABLE [dbo].[CL40_LIT_HST] (
    [BF_SSN]           CHAR (9)       NOT NULL,
    [LN_SEQ]           SMALLINT       NOT NULL,
    [LN_LIT_SEQ]       SMALLINT       NOT NULL,
    [LF_USR_LIT_UPD]   VARCHAR (8)    NOT NULL,
    [LC_LIT_STA]       VARCHAR (2)    NOT NULL,
    [LD_LIT_STA]       DATE           NOT NULL,
    [LF_LST_DTS_CL40]  DATETIME       NOT NULL,
    [LD_GTR_APV_1_PCA] DATE           NULL,
    [LD_GTR_APV_3_PCA] DATE           NULL,
    [LD_LIT_BEG]       DATE           NULL,
    [LA_PRI_PRT_HHS]   NUMERIC (8, 2) NULL,
    [LA_INT_RPT_HHS]   NUMERIC (7, 2) NULL,
    [LD_LIT_LON_DLQ]   DATE           NULL,
    CONSTRAINT [PK_CL40_LIT_HST] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [LN_LIT_SEQ] ASC) WITH (FILLFACTOR = 95)
);

