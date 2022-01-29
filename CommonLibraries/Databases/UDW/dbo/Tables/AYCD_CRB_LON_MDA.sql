﻿CREATE TABLE [dbo].[AYCD_CRB_LON_MDA] (
    [BF_SSN]            CHAR (9)      NOT NULL,
    [LN_SEQ]            SMALLINT      NOT NULL,
    [LF_CRT_DTS_AYCD]   DATETIME2 (7) NOT NULL,
    [LF_CRT_USR_AYCD]   VARCHAR (12)  NOT NULL,
    [LF_LST_DTS_AYCD]   DATETIME2 (7) NOT NULL,
    [LF_LST_USR_AYCD]   VARCHAR (12)  NOT NULL,
    [LN_DFL_SEQ]        SMALLINT      NULL,
    [LD_1_SYS_DLQ_CVN]  DATE          NULL,
    [LD_1_DLQ_CLC_SYS]  DATE          NULL,
    [LD_ACC_OPN]        DATE          NULL,
    [LC_LON_HAS_PAY_DU] CHAR (1)      NOT NULL,
    [LN_SPC_TRM_DUR]    VARCHAR (3)   NOT NULL,
    [LN_QA_RPT_SEQ]     BIGINT        NULL,
    [LC_TDL_PRG_REA]    CHAR (1)      NOT NULL,
    CONSTRAINT [PK_AYCD_CRB_LON_MDA] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC) WITH (FILLFACTOR = 95)
);

