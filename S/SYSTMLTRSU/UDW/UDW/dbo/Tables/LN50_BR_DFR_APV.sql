CREATE TABLE [dbo].[LN50_BR_DFR_APV] (
    [BF_SSN]             CHAR (9)      NOT NULL,
    [LN_SEQ]             SMALLINT      NOT NULL,
    [LF_DFR_CTL_NUM]     CHAR (3)      NOT NULL,
    [LN_DFR_OCC_SEQ]     SMALLINT      NOT NULL,
    [LC_DFR_RSP]         CHAR (3)      NOT NULL,
    [LD_DFR_BEG]         DATE          NULL,
    [LD_DFR_END]         DATE          NULL,
    [LD_DFR_GRC_END]     DATE          NULL,
    [LF_LST_DTS_LN50]    DATETIME2 (7) NOT NULL,
    [LC_STA_LON50]       CHAR (1)      NOT NULL,
    [LD_STA_LON50]       DATE          NULL,
    [LD_DFR_APL]         DATE          NULL,
    [LC_LON_LEV_DFR_CAP] CHAR (1)      NOT NULL,
    [LI_DLQ_CAP]         CHAR (1)      NOT NULL,
    CONSTRAINT [PK_LN50_BR_DFR_APV] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [LF_DFR_CTL_NUM] ASC, [LN_DFR_OCC_SEQ] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_BFSSN_LCSTALON50>]
    ON [dbo].[LN50_BR_DFR_APV]([BF_SSN] ASC, [LC_STA_LON50] ASC);

