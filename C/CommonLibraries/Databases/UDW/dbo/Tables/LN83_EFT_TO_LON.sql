﻿CREATE TABLE [dbo].[LN83_EFT_TO_LON] (
    [BF_SSN]             CHAR (9)       NOT NULL,
    [LN_SEQ]             SMALLINT       NOT NULL,
    [BN_EFT_SEQ]         SMALLINT       NOT NULL,
    [LF_EFT_OCC_DTS]     DATETIME2 (7)  NOT NULL,
    [LF_LST_DTS_LN83]    DATETIME2 (7)  NOT NULL,
    [LD_EFT_EFF_BEG]     DATE           NULL,
    [LD_EFT_EFF_END]     DATE           NULL,
    [LC_EFT_SUS_REA]     CHAR (1)       NOT NULL,
    [LC_STA_LN83]        CHAR (1)       NOT NULL,
    [LR_EFT_RDC]         NUMERIC (5, 3) NULL,
    [LI_EFT_RIR_MNL_OVR] CHAR (1)       NOT NULL,
    [LI_EFT_RST]         CHAR (1)       NOT NULL,
    [LF_LST_USR_LN83]    VARCHAR (12)   NOT NULL,
    [LF_LST_SRC_LN83]    VARCHAR (16)   NOT NULL,
    CONSTRAINT [PK_LN83_EFT_TO_LON] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [BN_EFT_SEQ] ASC, [LF_EFT_OCC_DTS] ASC) WITH (FILLFACTOR = 95)
);
