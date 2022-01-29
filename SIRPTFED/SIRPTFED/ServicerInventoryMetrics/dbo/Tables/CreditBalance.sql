﻿CREATE TABLE [dbo].[CreditBalance] (
    [CreditBalanceId] INT       IDENTITY (1, 1) NOT NULL,
    [DF_SPE_ACC_ID]   CHAR (10) NOT NULL,
    [LN_SEQ]          SMALLINT  NOT NULL,
    [LN_FAT_SEQ]      SMALLINT  NOT NULL,
    [RunningTotal]    MONEY     NOT NULL,
    [LD_FAT_APL]      DATETIME  NOT NULL,
    PRIMARY KEY CLUSTERED ([CreditBalanceId] ASC)
);

