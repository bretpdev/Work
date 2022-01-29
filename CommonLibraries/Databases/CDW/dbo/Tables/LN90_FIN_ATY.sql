﻿CREATE TABLE [dbo].[LN90_FIN_ATY] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [LN_SEQ]             SMALLINT        NOT NULL,
    [LN_FAT_SEQ]         SMALLINT        NOT NULL,
    [LC_FAT_REV_REA]     CHAR (1)        NOT NULL,
    [LD_FAT_APL]         DATE            NOT NULL,
    [LD_FAT_PST]         DATE            NULL,
    [LD_FAT_EFF]         DATE            NOT NULL,
    [LD_FAT_DPS]         DATE            NULL,
    [LC_CSH_ADV]         CHAR (1)        NOT NULL,
    [LD_STA_LON90]       DATE            NOT NULL,
    [LC_STA_LON90]       CHAR (1)        NOT NULL,
    [LA_FAT_PCL_FEE]     NUMERIC (7, 2)  NULL,
    [LA_FAT_NSI]         NUMERIC (7, 2)  NULL,
    [LA_FAT_LTE_FEE]     NUMERIC (7, 2)  NULL,
    [LA_FAT_ILG_PRI]     NUMERIC (8, 2)  NULL,
    [LA_FAT_CUR_PRI]     NUMERIC (8, 2)  NULL,
    [LF_LST_DTS_LN90]    DATETIME2 (7)   NOT NULL,
    [PC_FAT_TYP]         VARCHAR (2)     NOT NULL,
    [PC_FAT_SUB_TYP]     VARCHAR (2)     NOT NULL,
    [LA_FAT_NSI_ACR]     NUMERIC (7, 2)  NULL,
    [LI_FAT_RAP]         CHAR (1)        NOT NULL,
    [LN_FAT_SEQ_REV]     SMALLINT        NULL,
    [LI_EFT_NSF_OVR]     CHAR (1)        NOT NULL,
    [LF_USR_EFT_NSF_OVR] VARCHAR (8)     NOT NULL,
    [LA_FAT_MSC_FEE]     NUMERIC (12, 2) NULL,
    [LA_FAT_MSC_FEE_PCV] NUMERIC (12, 2) NULL,
    [LA_FAT_DL_REB]      NUMERIC (12, 2) NULL,
    [LA_GOV_INT_100_PER] NUMERIC (12, 2) NULL,
    [LA_RPA_CTH_FAT_ADJ] NUMERIC (12, 2) NULL,
    [LA_FAT_NSI_CAP]     NUMERIC (12, 2) NULL,
    [LA_FAT_NSI_NCA]     NUMERIC (12, 2) NULL,
    [LA_FAT_NSI_NAM]     NUMERIC (12, 2) NULL,
    [LA_FAT_NSI_ACR_CAP] NUMERIC (12, 2) NULL,
    [LA_FAT_NSI_ACR_NCA] NUMERIC (12, 2) NULL,
    [LA_FAT_NSI_ACR_NAM] NUMERIC (12, 2) NULL
);


GO
CREATE CLUSTERED INDEX [CIX_LD_STA_LON90]
    ON [dbo].[LN90_FIN_ATY]([LD_STA_LON90] ASC, [BF_SSN] ASC, [LN_SEQ] ASC, [LN_FAT_SEQ] ASC) WITH (FILLFACTOR = 95);

