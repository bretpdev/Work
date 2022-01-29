﻿CREATE TABLE [dbo].[LN90_FIN_ATY] (
    [BF_SSN]             VARCHAR (9)     NOT NULL,
    [LN_SEQ]             SMALLINT        NOT NULL,
    [LN_FAT_SEQ]         SMALLINT        NOT NULL,
    [LC_FAT_REV_REA]     CHAR (1)        NULL,
    [LD_FAT_APL]         DATETIME        NULL,
    [LD_FAT_PST]         DATETIME        NULL,
    [LD_FAT_EFF]         DATETIME        NULL,
    [LD_FAT_DPS]         DATETIME        NULL,
    [LC_CSH_ADV]         CHAR (1)        NULL,
    [LD_STA_LON90]       DATETIME        NULL,
    [LC_STA_LON90]       CHAR (1)        NULL,
    [LA_FAT_PCL_FEE]     DECIMAL (7, 2)  NULL,
    [LA_FAT_NSI]         DECIMAL (7, 2)  NULL,
    [LA_FAT_LTE_FEE]     DECIMAL (7, 2)  NULL,
    [LA_FAT_ILG_PRI]     DECIMAL (8, 2)  NULL,
    [LA_FAT_CUR_PRI]     DECIMAL (8, 2)  NULL,
    [LF_LST_DTS_LN90]    DATETIME        NULL,
    [PC_FAT_TYP]         VARCHAR (2)     NULL,
    [PC_FAT_SUB_TYP]     VARCHAR (2)     NULL,
    [LA_FAT_NSI_ACR]     DECIMAL (7, 2)  NULL,
    [LI_FAT_RAP]         CHAR (1)        NULL,
    [LN_FAT_SEQ_REV]     SMALLINT        NULL,
    [LI_EFT_NSF_OVR]     CHAR (1)        NULL,
    [LF_USR_EFT_NSF_OVR] VARCHAR (8)     NULL,
    [LA_FAT_MSC_FEE]     DECIMAL (12, 2) NULL,
    [LA_FAT_MSC_FEE_PCV] DECIMAL (12, 2) NULL,
    [LA_FAT_DL_REB]      DECIMAL (12, 2) NULL,
    PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [LN_FAT_SEQ] ASC)
);

