﻿CREATE TABLE [dbo].[GRS2_NDS_DSB_RSP] (
    [DF_PRS_ID]          CHAR (9)       NOT NULL,
    [LF_NDS_LON_RSP_LAB] VARCHAR (17)   NOT NULL,
    [LD_NDS_LIS_REQ]     DATE           NOT NULL,
    [LN_LIS_DSB_SEQ]     SMALLINT       NOT NULL,
    [LF_CRT_DTS_GRS2]    DATETIME2 (7)  NOT NULL,
    [LF_CRT_USR_GRS2]    VARCHAR (12)   NOT NULL,
    [LF_CRT_SRC_GRS2]    VARCHAR (16)   NOT NULL,
    [LD_DSB]             DATE           NULL,
    [LA_DSB]             NUMERIC (8, 2) NOT NULL,
    CONSTRAINT [PK_GRS2_NDS_DSB_RSP] PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [LF_NDS_LON_RSP_LAB] ASC, [LD_NDS_LIS_REQ] ASC, [LN_LIS_DSB_SEQ] ASC) WITH (FILLFACTOR = 95)
);
