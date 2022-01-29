﻿CREATE TABLE [dbo].[LN94_LON_PAY_FAT] (
    [BF_SSN]             VARCHAR (9)   NOT NULL,
    [LN_SEQ]             SMALLINT      NOT NULL,
    [LN_FAT_SEQ]         SMALLINT      NOT NULL,
    [LD_RMT_BCH_INI]     DATE          NOT NULL,
    [LC_RMT_BCH_SRC_IPT] CHAR (1)      NOT NULL,
    [LN_RMT_BCH_SEQ]     SMALLINT      NOT NULL,
    [LN_RMT_SEQ]         NUMERIC (6)   NOT NULL,
    [LN_RMT_ITM]         NUMERIC (3)   NOT NULL,
    [LN_RMT_ITM_SEQ]     SMALLINT      NOT NULL,
    [LF_LST_DTS_LN94]    DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_LN94_LON_PAY_FAT] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [LN_FAT_SEQ] ASC, [LD_RMT_BCH_INI] ASC, [LC_RMT_BCH_SRC_IPT] ASC, [LN_RMT_BCH_SEQ] ASC, [LN_RMT_SEQ] ASC, [LN_RMT_ITM] ASC, [LN_RMT_ITM_SEQ] ASC) WITH (FILLFACTOR = 95)
);
