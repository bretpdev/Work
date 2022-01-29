﻿CREATE TABLE [dbo].[RP30_EFT_RIR_PAR] (
    [IF_OWN]             VARCHAR (8)    NOT NULL,
    [PN_EFT_RIR_OWN_SEQ] SMALLINT       NOT NULL,
    [IC_LON_PGM]         VARCHAR (6)    NULL,
    [IF_GTR]             VARCHAR (6)    NULL,
    [PD_LON_1_DSB]       DATE           NULL,
    [PF_DOE_SCL_ORG]     VARCHAR (8)    NULL,
    [PC_ST_BR_RSD_APL]   VARCHAR (2)    NOT NULL,
    [PD_EFT_RIR_EFF_BEG] DATE           NULL,
    [PD_EFT_RIR_EFF_END] DATE           NULL,
    [PC_EFT_RIR_STA]     CHAR (1)       NOT NULL,
    [PD_EFT_RIR_STA]     DATE           NULL,
    [PI_EFT_RIR_PRC]     CHAR (1)       NOT NULL,
    [PC_EFT_NSF_LTR_REQ] VARCHAR (5)    NULL,
    [PR_EFT_RIR]         NUMERIC (5, 3) NULL,
    [PF_LST_USR_RP30]    VARCHAR (8)    NOT NULL,
    [PF_LST_DTS_RP30]    DATETIME2 (7)  NOT NULL,
    [PC_EFT_RIR_PNT_YR]  VARCHAR (7)    NOT NULL,
    [PD_EFT_BBS_LOT_BEG] DATE           NULL,
    [PD_EFT_BBS_GTE_DTE] DATE           NULL,
    [PD_EFT_BBS_RPD_SR]  DATE           NULL,
    [PD_EFT_BBS_LCO_RCV] DATE           NULL,
    [PN_EFT_BBS_NSF_LMT] NUMERIC (3)    NULL,
    [PC_EFT_BBS_NSF_PRC] CHAR (1)       NOT NULL,
    [PN_EFT_BBS_NSF_MTH] NUMERIC (3)    NULL,
    [PC_EFT_BBS_FED]     VARCHAR (3)    NOT NULL,
    [PI_EFT_RIR_RPY_0]   CHAR (1)       NOT NULL,
    CONSTRAINT [PK_RP30_EFT_RIR_PAR] PRIMARY KEY CLUSTERED ([IF_OWN] ASC, [PN_EFT_RIR_OWN_SEQ] ASC) WITH (FILLFACTOR = 95)
);

