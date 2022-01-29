﻿CREATE TABLE [dbo].[LN60_BR_FOR_APV] (
    [BF_SSN]             VARCHAR (9)    NOT NULL,
    [LN_SEQ]             SMALLINT       NOT NULL,
    [LF_FOR_CTL_NUM]     VARCHAR (3)    NOT NULL,
    [LN_FOR_OCC_SEQ]     SMALLINT       NOT NULL,
    [LC_FOR_RSP]         VARCHAR (3)    NULL,
    [LD_FOR_BEG]         DATETIME       NULL,
    [LD_FOR_END]         DATETIME       NULL,
    [LD_STA_LON60]       DATETIME       NULL,
    [LC_STA_LON60]       CHAR (1)       NULL,
    [LD_FOR_APL]         DATETIME       NULL,
    [LF_LST_DTS_LN60]    DATETIME       NULL,
    [LI_FOR_20_RPT]      CHAR (1)       NULL,
    [LC_LON_LEV_FOR_CAP] CHAR (1)       NULL,
    [LA_FOR_20_INT_ACR]  DECIMAL (9, 2) NULL,
    [LA_ACL_RDC_PAY]     DECIMAL (7, 2) NULL,
    [LI_FOR_VRB_DFL_RUL] CHAR (1)       NULL,
    PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [LF_FOR_CTL_NUM] ASC, [LN_FOR_OCC_SEQ] ASC)
);

