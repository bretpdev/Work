﻿CREATE TABLE [dbo].[LN55_LON_BBS_TIR] (
    [BF_SSN]             CHAR (9)      NOT NULL,
    [LN_SEQ]             SMALLINT      NOT NULL,
    [LN_LON_BBS_SEQ]     SMALLINT      NOT NULL,
    [LN_LON_BBT_SEQ]     SMALLINT      NOT NULL,
    [LF_LON_BBS_TIR]     VARCHAR (2)   NOT NULL,
    [LF_LON_BBS_SUB_TIR] VARCHAR (2)   NOT NULL,
    [PM_BBS_PGM]         VARCHAR (3)   NOT NULL,
    [PN_BBS_PGM_SEQ]     SMALLINT      NOT NULL,
    [PF_BBS_PGM_TIR]     VARCHAR (2)   NOT NULL,
    [PN_BBS_PGM_TIR_SEQ] SMALLINT      NOT NULL,
    [LN_BR_DSB_SEQ]      SMALLINT      NULL,
    [LC_STA_LN55]        CHAR (1)      NOT NULL,
    [LD_STA_LN55]        DATE          NULL,
    [LD_LON_BBT_CHK_ISS] DATE          NULL,
    [LD_BBT_DSQ_OVR_END] DATE          NULL,
    [LN_LON_BBT_PAY_OVR] NUMERIC (3)   NULL,
    [LD_LON_BBT_BEG]     DATE          NULL,
    [LD_REB_MTD_LTR_SNT] DATE          NULL,
    [LC_LON_BBT_REB_MTD] CHAR (1)      NOT NULL,
    [LN_BBT_PAY_PIF_MOT] NUMERIC (3)   NULL,
    [LN_BBT_PAY_DLQ_MOT] NUMERIC (3)   NULL,
    [LC_LON_BBT_DSQ_REA] VARCHAR (2)   NOT NULL,
    [LC_LON_BBT_STA]     CHAR (1)      NOT NULL,
    [LN_LON_BBT_PAY]     NUMERIC (3)   NULL,
    [LD_LON_BBT_STA]     DATE          NULL,
    [LD_BBT_STS_PAY]     DATE          NULL,
    [LF_LST_USR_LN55]    VARCHAR (8)   NOT NULL,
    [LF_LST_DTS_LN55]    DATETIME2 (7) NOT NULL,
    [LD_LON_BBT_ELG_FNL] DATE          NULL,
    [LD_BBT_DLQ_MOT_STS] DATE          NULL,
    [LD_BBT_PIF_MOT_STS] DATE          NULL,
    [LN_BBT_DLQ_MOT_OVR] NUMERIC (3)   NULL,
    [LN_BBT_PIF_MOT_OVR] NUMERIC (3)   NULL,
    CONSTRAINT [PK_LN55_LON_BBS_TIR] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [LN_LON_BBS_SEQ] ASC, [LN_LON_BBT_SEQ] ASC) WITH (FILLFACTOR = 95)
);
