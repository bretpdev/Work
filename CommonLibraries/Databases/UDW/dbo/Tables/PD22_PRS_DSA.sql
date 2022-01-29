﻿CREATE TABLE [dbo].[PD22_PRS_DSA] (
    [DF_PRS_ID]          VARCHAR (9)  NOT NULL,
    [DD_DSA_RPT]         DATETIME     NOT NULL,
    [DD_DSA]             DATETIME     NULL,
    [DF_DR]              VARCHAR (9)  NULL,
    [DC_DSA_NEW_WRS]     CHAR (1)     NULL,
    [DF_LST_DTS_PD22]    DATETIME     NULL,
    [DI_DSA_VET]         CHAR (1)     NULL,
    [DD_PRS_DSA_SPS_SR]  DATETIME     NULL,
    [DD_PRS_DSA_SPS_END] DATETIME     NULL,
    [DF_CRT_USR_PD22]    VARCHAR (8)  NULL,
    [DF_LST_USR_PD22]    VARCHAR (8)  NULL,
    [DD_REC_LST_UPD]     DATETIME     NULL,
    [DX_PRS_DSA_TPD_REA] VARCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [DD_DSA_RPT] ASC)
);

