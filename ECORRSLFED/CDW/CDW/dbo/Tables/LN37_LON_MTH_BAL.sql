﻿CREATE TABLE [dbo].[LN37_LON_MTH_BAL] (
    [BF_SSN]             VARCHAR (9)     NOT NULL,
    [LN_SEQ]             SMALLINT        NOT NULL,
    [LD_EFF_MTH_BAL]     DATE            NOT NULL,
    [LN_MTH_BAL_SEQ]     SMALLINT        NOT NULL,
    [LD_APL_MTH_BAL]     DATE            NOT NULL,
    [LA_OTS_PRI_ELG]     NUMERIC (8, 2)  NOT NULL,
    [LA_OTS_PRI_ILG]     NUMERIC (8, 2)  NOT NULL,
    [LA_NSI_ACR]         NUMERIC (12, 7) NOT NULL,
    [LA_NSI_PD]          NUMERIC (7, 2)  NOT NULL,
    [LA_OTS_LTE_FEE]     NUMERIC (7, 2)  NOT NULL,
    [LF_LST_DTS_LN37]    DATETIME2 (7)   NOT NULL,
    [LD_EOM_BAL_RPT]     DATE            NULL,
    [LI_TL4_MTH_BAL]     CHAR (1)        NOT NULL,
    [LD_EOQ_BAL_RPT]     DATE            NULL,
    [IF_BND_ISS]         VARCHAR (8)     NULL,
    [IF_OWN]             VARCHAR (8)     NOT NULL,
    [LC_STA_LON37]       CHAR (1)        NOT NULL,
    [LA_GOV_INT_RPT]     NUMERIC (7, 2)  NULL,
    [LD_STA_LN37]        DATE            NULL,
    [LA_MTH_AVG_DAY_BAL] NUMERIC (8, 2)  NULL,
    [LC_PGM_GOV_INT_RPT] CHAR (1)        NOT NULL,
    [LA_ACR_MTH_INT]     NUMERIC (8, 2)  NULL,
    CONSTRAINT [PK_LN37_LON_MTH_BAL] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [LD_EFF_MTH_BAL] ASC, [LN_MTH_BAL_SEQ] ASC) WITH (FILLFACTOR = 95)
);

