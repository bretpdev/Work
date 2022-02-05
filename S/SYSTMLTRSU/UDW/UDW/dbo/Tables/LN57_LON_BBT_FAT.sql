﻿CREATE TABLE [dbo].[LN57_LON_BBT_FAT] (
    [BF_SSN]          VARCHAR (9)   NOT NULL,
    [LN_SEQ]          SMALLINT      NOT NULL,
    [LN_LON_BBS_SEQ]  SMALLINT      NOT NULL,
    [LN_LON_BBT_SEQ]  SMALLINT      NOT NULL,
    [LN_FAT_SEQ]      SMALLINT      NOT NULL,
    [LF_LST_USR_LN57] VARCHAR (8)   NOT NULL,
    [LF_LST_DTS_LN57] DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_LN57_LON_BBT_FAT] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [LN_LON_BBS_SEQ] ASC, [LN_LON_BBT_SEQ] ASC, [LN_FAT_SEQ] ASC) WITH (FILLFACTOR = 95)
);
