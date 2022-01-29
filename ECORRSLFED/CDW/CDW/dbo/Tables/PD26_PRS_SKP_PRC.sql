CREATE TABLE [dbo].[PD26_PRS_SKP_PRC] (
    [DF_PRS_ID]       CHAR (9)      NOT NULL,
    [BF_SSN]          CHAR (9)      NOT NULL,
    [LN_SEQ]          SMALLINT      NOT NULL,
    [DN_SKP_PRS_SEQ]  SMALLINT      NOT NULL,
    [DD_SKP_END]      DATE          NULL,
    [DD_SKP_BEG]      DATE          NULL,
    [DC_SKP_TYP]      CHAR (1)      NOT NULL,
    [DD_SKP_EFF_OCC]  DATE          NOT NULL,
    [DC_SKP_PRS]      CHAR (1)      NOT NULL,
    [DC_STA_SKP]      CHAR (1)      NOT NULL,
    [DF_LST_DTS_PD26] DATETIME2 (7) NOT NULL,
    [DD_STA_SKP]      DATE          NOT NULL,
    CONSTRAINT [PK_PD26_PRS_SKP_PRC] PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [BF_SSN] ASC, [LN_SEQ] ASC, [DN_SKP_PRS_SEQ] ASC)
);

