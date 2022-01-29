﻿CREATE TABLE [dbo].[DW01_DW_CLC_CLU] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [LN_SEQ]             SMALLINT        NOT NULL,
    [WD_CLC_THU]         DATE            NOT NULL,
    [LA_NSI_OTS]         NUMERIC (12, 2) NULL,
    [LA_NSI_ACR]         NUMERIC (12, 2) NULL,
    [WA_TOT_BRI_OTS]     NUMERIC (12, 2) NULL,
    [WC_DW_LON_STA]      CHAR (2)        NOT NULL,
    [WD_LON_RPD_SR]      DATE            NULL,
    [WD_XPC_POF_TS26]    DATE            NULL,
    [WX_OVR_DW_LON_STA]  CHAR (20)       NULL,
    [WA_STD_STD_ISL]     NUMERIC (12, 2) NULL,
    [WC_LON_STA_GRC]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_SCL]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_RPY]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_DFR]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_FOR]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_CUR]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_CLM]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_PCL]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_DTH]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_DSA]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_BKR]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_PIF]     VARCHAR (2)     NOT NULL,
    [WC_LON_STA_FUL_ORG] VARCHAR (2)     NOT NULL,
    [WC_LON_DFR_FOR_TYP] VARCHAR (2)     NOT NULL,
    CONSTRAINT [PK_DW01_DW_CLC_CLU] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_BFSSN_LNSEQ]
    ON [dbo].[DW01_DW_CLC_CLU]([BF_SSN] ASC, [LN_SEQ] ASC)
    INCLUDE([WA_TOT_BRI_OTS]);

