CREATE TABLE [dbo].[PD21_GTR_DTH] (
    [DF_PRS_ID]       CHAR (9)      NOT NULL,
    [DD_DTH_NTF]      DATE          NOT NULL,
    [IF_GTR]          CHAR (6)      NOT NULL,
    [DD_DTH_VER]      DATE          NULL,
    [DC_DTH_STA]      CHAR (2)      NOT NULL,
    [DC_DTH_CER]      CHAR (2)      NOT NULL,
    [DF_LST_DTS_PD21] DATETIME2 (7) NOT NULL,
    [DD_DTH_STA]      DATE          NOT NULL,
    CONSTRAINT [PK_PD21_GTR_DTH] PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [DD_DTH_NTF] ASC, [IF_GTR] ASC) WITH (FILLFACTOR = 95)
);

