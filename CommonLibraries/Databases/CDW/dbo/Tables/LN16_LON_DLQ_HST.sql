﻿CREATE TABLE [dbo].[LN16_LON_DLQ_HST] (
    [BF_SSN]             CHAR (9)    NOT NULL,
    [LN_SEQ]             SMALLINT    NOT NULL,
    [LN_DLQ_SEQ]         SMALLINT    NOT NULL,
    [LC_DLQ_TYP]         CHAR (1)    NOT NULL,
    [LC_STA_LON16]       CHAR (1)    NOT NULL,
    [LD_STA_LON16]       DATETIME    NOT NULL,
    [LD_DLQ_OCC]         DATETIME    NULL,
    [LN_DLQ_MAX]         NUMERIC (4) NOT NULL,
    [LN_DLQ_ITL]         NUMERIC (4) NOT NULL,
    [LD_DLQ_MAX]         DATETIME    NOT NULL,
    [LD_DLQ_ITL]         DATETIME    NOT NULL,
    [LF_LST_DTS_LN16]    DATETIME    NOT NULL,
    [LI_RSM_DUN_NXT_BKT] CHAR (1)    NOT NULL,
    [LI_NO_DLQ_IC]       CHAR (1)    NOT NULL
);

