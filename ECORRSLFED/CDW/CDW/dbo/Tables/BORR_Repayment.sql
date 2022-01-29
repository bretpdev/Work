﻿CREATE TABLE [dbo].[BORR_Repayment] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [LD_CRT_LON65]  VARCHAR (30) NULL,
    [DUE_DAY]       VARCHAR (30) NULL,
    [MONTH_AMT]     VARCHAR (30) NULL,
    [MULT_DUE_DT]   VARCHAR (1)  NULL,
    CONSTRAINT [PK_BORR_Repayment] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC) WITH (FILLFACTOR = 95)
);

