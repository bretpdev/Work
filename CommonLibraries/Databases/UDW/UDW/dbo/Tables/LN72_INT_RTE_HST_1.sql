﻿CREATE TABLE [dbo].[LN72_INT_RTE_HST] (
    [BF_SSN]             CHAR (9)       NOT NULL,
    [LN_SEQ]             SMALLINT       NOT NULL,
    [LC_ITR_TYP]         VARCHAR (2)    NOT NULL,
    [LN_ITR_SEQ]         SMALLINT       NOT NULL,
    [LD_ITR_EFF_BEG]     DATE           NULL,
    [LC_ELG_SIN]         CHAR (1)       NOT NULL,
    [LC_STA_LON72]       CHAR (1)       NOT NULL,
    [LD_CRT_LON72]       DATE           NOT NULL,
    [LD_ITR_APL]         DATE           NOT NULL,
    [LD_STA_LON72]       DATE           NOT NULL,
    [LI_SPC_ITR]         CHAR (1)       NOT NULL,
    [LD_ITR_EFF_END]     DATE           NULL,
    [LR_ITR]             NUMERIC (5, 3) NOT NULL,
    [LF_LST_DTS_LN72]    DATETIME2 (7)  NOT NULL,
    [LC_ROW_SRC_TP_MNL]  CHAR (1)       NOT NULL,
    [LC_INT_RDC_PGM]     CHAR (1)       NOT NULL,
    [LR_INT_RDC_PGM_ORG] NUMERIC (5, 3) NULL,
    [LR_SCRA_CAP]        NUMERIC (5, 3) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_SSN_SEQ_ITR_TYP_ITR_SEQ]
    ON [dbo].[LN72_INT_RTE_HST]([BF_SSN] ASC, [LN_SEQ] ASC, [LC_ITR_TYP] ASC, [LN_ITR_SEQ] ASC) WITH (FILLFACTOR = 95, PAD_INDEX = ON);

