﻿CREATE TABLE [dbo].[LN35_LON_OWN] (
    [BF_SSN]             VARCHAR (9)    NOT NULL,
    [LN_SEQ]             SMALLINT       NOT NULL,
    [IF_OWN]             VARCHAR (8)    NOT NULL,
    [LN_LON_OWN_SEQ]     SMALLINT       NOT NULL,
    [LD_OWN_EFF_SR]      DATETIME       NULL,
    [LF_BR_LON_OWN_ACC]  VARCHAR (17)   NULL,
    [LF_CUR_POR]         VARCHAR (20)   NULL,
    [LD_OWN_EFF_END]     DATETIME       NULL,
    [LF_OWN_ORG_POR]     VARCHAR (20)   NULL,
    [LC_LOC_PNT]         VARCHAR (3)    NULL,
    [LC_STA_LON35]       CHAR (1)       NULL,
    [LF_LST_DTS_LN35]    DATETIME       NULL,
    [IF_BND_ISS]         VARCHAR (8)    NULL,
    [IF_LON_SLE]         VARCHAR (7)    NULL,
    [IF_TIR_PCE]         VARCHAR (3)    NULL,
    [LD_LON_IRL_SLE_TRF] DATETIME       NULL,
    [LI_ORG_RGT_PUR_SLE] CHAR (1)       NULL,
    [LF_OWN_EFT_RIR_ASN] VARCHAR (8)    NULL,
    [LA_LON_LVL_TRF_FEE] DECIMAL (7, 2) NULL,
    [LD_PRE_CVN_OWN_BEG] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [IF_OWN] ASC, [LN_LON_OWN_SEQ] ASC)
);

