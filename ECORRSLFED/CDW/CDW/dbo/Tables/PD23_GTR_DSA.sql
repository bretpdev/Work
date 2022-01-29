CREATE TABLE [dbo].[PD23_GTR_DSA] (
    [DF_PRS_ID]       CHAR (9)      NOT NULL,
    [DD_DSA_RPT]      DATE          NOT NULL,
    [IF_GTR]          CHAR (6)      NOT NULL,
    [DD_DSA_VER]      DATE          NULL,
    [DD_SBM_CLM_REQ]  DATE          NULL,
    [DD_DSA_APV]      DATE          NULL,
    [DI_DSA_XTN_REQ]  CHAR (1)      NOT NULL,
    [DI_DSA_APV]      CHAR (1)      NOT NULL,
    [DC_DSA_STA]      CHAR (2)      NOT NULL,
    [DF_LST_DTS_PD23] DATETIME2 (7) NOT NULL,
    [DD_DSA_STA]      DATE          NOT NULL,
    CONSTRAINT [PK_PD23_GTR_DSA] PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [DD_DSA_RPT] ASC, [IF_GTR] ASC) WITH (FILLFACTOR = 95)
);

