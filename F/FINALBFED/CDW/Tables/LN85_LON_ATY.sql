﻿CREATE TABLE [dbo].[LN85_LON_ATY] (
    [BF_SSN]          CHAR (9)      NOT NULL,
    [LN_SEQ]          SMALLINT      NOT NULL,
    [LN_ATY_SEQ]      INT           NOT NULL,
    [LF_LST_DTS_LN85] DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_LN85_LON_ATY] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [LN_ATY_SEQ] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_BFSSN]
    ON [dbo].[LN85_LON_ATY]([BF_SSN] ASC)
    INCLUDE([LN_SEQ], [LN_ATY_SEQ]);
