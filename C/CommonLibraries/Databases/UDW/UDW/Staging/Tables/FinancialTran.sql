CREATE TABLE [Staging].[FinancialTran] (
    [DF_SPE_ACC_ID]  VARCHAR (10)    NULL,
    [LN_SEQ]         INT             NULL,
    [LN_FAT_SEQ]     INT             NULL,
    [LD_FAT_PST]     VARCHAR (10)    NULL,
    [LD_FAT_EFF]     VARCHAR (10)    NULL,
    [LC_STA_LON90]   VARCHAR (1)     NULL,
    [LA_FAT_CUR_PRI] DECIMAL (28, 2) NULL,
    [LA_FAT_LTE_FEE] DECIMAL (28, 2) NULL,
    [PC_FAT_TYP]     VARCHAR (2)     NULL,
    [PC_FAT_SUB_TYP] VARCHAR (2)     NULL,
    [LA_FAT_NSI]     DECIMAL (28, 2) NULL,
    [LC_FAT_REV_REA] VARCHAR (1)     NULL,
    [FAT_REV_REA]    VARCHAR (37)    NULL
);

