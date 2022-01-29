﻿CREATE TABLE [dbo].[RF10_RFR] (
    [BF_SSN]             CHAR (9)      NOT NULL,
    [BN_SEQ_RFR]         SMALLINT      NOT NULL,
    [BC_STA_REFR10]      CHAR (1)      NOT NULL,
    [BI_ATH_3_PTY]       CHAR (1)      NOT NULL,
    [BC_RFR_REL_BR]      CHAR (2)      NOT NULL,
    [BD_EFF_RFR]         DATE          NULL,
    [BF_RFR]             VARCHAR (9)   NOT NULL,
    [BC_RFR_TYP]         CHAR (1)      NOT NULL,
    [BF_LST_DTS_RF10]    DATETIME2 (7) NOT NULL,
    [BD_EFF_RFR_HST]     DATE          NULL,
    [BC_REA_RFR_HST]     CHAR (1)      NOT NULL,
    [BF_LST_USR_HST_RFR] VARCHAR (8)   NOT NULL,
    [BD_ATH_3_PTY_END]   DATE          NULL,
    CONSTRAINT [PK_RF10_RFR] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [BN_SEQ_RFR] ASC) WITH (FILLFACTOR = 95)
);




GO
CREATE NONCLUSTERED INDEX [IX_RF10_RFR_BC_STA_REFR10_BF_RFR]
    ON [dbo].[RF10_RFR]([BC_STA_REFR10] ASC, [BF_RFR] ASC) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);
