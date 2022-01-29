﻿CREATE TABLE [dbo].[PD10_PRS_NME] (
    [DF_PRS_ID]        CHAR (9)      NOT NULL,
    [DD_STA_PRS]       DATE          NULL,
    [DC_LAG_FGN]       CHAR (1)      NOT NULL,
    [DC_SEX]           CHAR (1)      NOT NULL,
    [DD_BRT]           DATE          NULL,
    [DM_PRS_MID]       VARCHAR (13)  NOT NULL,
    [DM_PRS_1]         VARCHAR (13)  NOT NULL,
    [DM_PRS_LST_SFX]   VARCHAR (4)   NOT NULL,
    [DM_PRS_LST]       VARCHAR (23)  NOT NULL,
    [DD_DRV_LIC_REN]   DATE          NULL,
    [DC_ST_DRV_LIC]    VARCHAR (2)   NOT NULL,
    [DF_DRV_LIC]       VARCHAR (20)  NOT NULL,
    [DD_NME_VER_LST]   DATE          NULL,
    [DI_ORG_HLD]       CHAR (1)      NOT NULL,
    [DF_LST_USR_PD10]  VARCHAR (8)   NOT NULL,
    [DF_ALN_RGS]       VARCHAR (9)   NOT NULL,
    [DI_US_CTZ]        CHAR (1)      NOT NULL,
    [DF_LST_DTS_PD10]  DATETIME2 (7) NOT NULL,
    [DF_SPE_ACC_ID]    VARCHAR (10)  NOT NULL,
    [DF_PRS_LST_4_SSN] VARCHAR (4)   NOT NULL,
    [DI_ATU_FMT]       CHAR (1)      NOT NULL,
    [DC_ATU_FMT_TYP]   VARCHAR (2)   NOT NULL,
    CONSTRAINT [PK_PD10_PRS_NME] PRIMARY KEY NONCLUSTERED ([DF_PRS_ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_DFSPEACCID]
    ON [dbo].[PD10_PRS_NME]([DF_SPE_ACC_ID] ASC)
    INCLUDE([DF_PRS_ID]);


GO
CREATE CLUSTERED INDEX [CIX_DFPRSID]
    ON [dbo].[PD10_PRS_NME]([DF_PRS_ID] ASC);

