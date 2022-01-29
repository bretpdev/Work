﻿CREATE TABLE [dbo].[FR04_FMS_LON_RMT] (
    [WD_FMS_RPT]         DATE          NOT NULL,
    [WN_FMS_RPT_SEQ]     NUMERIC (6)   NOT NULL,
    [LD_RMT_BCH_INI]     DATE          NOT NULL,
    [LC_RMT_BCH_SRC_IPT] CHAR (1)      NOT NULL,
    [LN_RMT_BCH_SEQ]     SMALLINT      NOT NULL,
    [LN_RMT_SEQ]         NUMERIC (6)   NOT NULL,
    [LN_RMT_ITM]         NUMERIC (3)   NOT NULL,
    [LN_RMT_ITM_SEQ]     SMALLINT      NOT NULL,
    [WF_LST_DTS_FR04]    DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_FR04_FMS_LON_RMT] PRIMARY KEY CLUSTERED ([WD_FMS_RPT] ASC, [WN_FMS_RPT_SEQ] ASC, [LD_RMT_BCH_INI] ASC, [LC_RMT_BCH_SRC_IPT] ASC, [LN_RMT_BCH_SEQ] ASC, [LN_RMT_SEQ] ASC, [LN_RMT_ITM] ASC, [LN_RMT_ITM_SEQ] ASC) WITH (FILLFACTOR = 95)
);

