CREATE TABLE [dbo].[PD22_PRS_DSA] (
    [DF_PRS_ID]          CHAR (9)      NOT NULL,
    [DD_DSA_RPT]         DATE          NOT NULL,
    [DD_DSA]             DATE          NULL,
    [DF_DR]              CHAR (9)      NULL,
    [DC_DSA_NEW_WRS]     CHAR (1)      NOT NULL,
    [DF_LST_DTS_PD22]    DATETIME2 (7) NOT NULL,
    [DI_DSA_VET]         CHAR (1)      NOT NULL,
    [DD_PRS_DSA_SPS_SR]  DATE          NULL,
    [DD_PRS_DSA_SPS_END] DATE          NULL,
    [DF_CRT_USR_PD22]    VARCHAR (8)   NOT NULL,
    [DF_LST_USR_PD22]    VARCHAR (8)   NOT NULL,
    [DD_REC_LST_UPD]     DATE          NULL,
    [DX_PRS_DSA_TPD_REA] VARCHAR (10)  NOT NULL,
    [DI_DSA_VET_ADM_MCH] CHAR (1)      NOT NULL,
    CONSTRAINT [PK_PD22_PRS_DSA] PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [DD_DSA_RPT] ASC) WITH (FILLFACTOR = 95)
);

