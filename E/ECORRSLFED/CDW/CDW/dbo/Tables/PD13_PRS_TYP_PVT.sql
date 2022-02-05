CREATE TABLE [dbo].[PD13_PRS_TYP_PVT] (
    [DF_PRS_ID]          CHAR (9)      NOT NULL,
    [DF_OWN_PVT_POL_LTR] CHAR (10)     NOT NULL,
    [DF_OWN_PVT_POL_ENC] CHAR (8)      NOT NULL,
    [DC_PVT_POL_FNC]     CHAR (1)      NOT NULL,
    [DN_SEQ_PD12]        SMALLINT      NOT NULL,
    [DN_SEQ_PD13]        SMALLINT      NOT NULL,
    [DD_CRT_PD13]        DATE          NOT NULL,
    [DC_PRS_TYP]         CHAR (1)      NOT NULL,
    [IF_OWN]             CHAR (8)      NOT NULL,
    [BF_SSN]             CHAR (9)      NOT NULL,
    [LN_SEQ]             SMALLINT      NULL,
    [AN_SEQ]             SMALLINT      NULL,
    [DC_STA_PD13]        CHAR (1)      NOT NULL,
    [DD_STA_PD13]        DATE          NULL,
    [DF_LST_DTS_PD13]    DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_PD13_PRS_TYP_PVT] PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [DF_OWN_PVT_POL_LTR] ASC, [DF_OWN_PVT_POL_ENC] ASC, [DC_PVT_POL_FNC] ASC, [DN_SEQ_PD12] ASC, [DN_SEQ_PD13] ASC) WITH (FILLFACTOR = 95)
);

