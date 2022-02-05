﻿CREATE TABLE [dbo].[RP05_BBS_DFR_FOR] (
    [PM_BBS_PGM]         VARCHAR (3) NOT NULL,
    [PN_BBS_PGM_SEQ]     SMALLINT    NOT NULL,
    [PN_BBS_DFR_FOR_SEQ] SMALLINT    NOT NULL,
    [PC_BBS_DFR_FOR]     CHAR (1)    NULL,
    [PC_BBS_DFR_FOR_TYP] VARCHAR (2) NULL,
    [PC_BBS_DFR_FOR_EFE] CHAR (1)    NULL,
    [PC_STA_RP05]        CHAR (1)    NULL,
    [PD_STA_RP05]        DATETIME    NULL,
    [PF_LST_USR_RP05]    VARCHAR (8) NULL,
    [PF_LST_DTS_RP05]    DATETIME    NULL,
    PRIMARY KEY CLUSTERED ([PM_BBS_PGM] ASC, [PN_BBS_PGM_SEQ] ASC, [PN_BBS_DFR_FOR_SEQ] ASC) WITH (FILLFACTOR = 95)
);
