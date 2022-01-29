﻿CREATE TABLE [dbo].[PH05_ContactEmail] (
    [DF_SPE_ACC_ID]      VARCHAR (10)  NOT NULL,
    [DX_CNC_EML_ADR]     VARCHAR (254) NOT NULL,
    [DF_DTS_EML_ADR_EFF] DATETIME      NOT NULL,
    [DF_LST_USR_EML_ADR] VARCHAR (8)   NOT NULL,
    [DI_VLD_CNC_EML_ADR] BIT           NOT NULL,
    [DI_CNC_ELT_OPI]     BIT           NOT NULL,
    [DC_ELT_OPI_SRC]     CHAR (2)      NOT NULL,
    [DI_CNC_EBL_OPI]    BIT           NOT NULL,
    [DC_EBL_OPI_SRC]     CHAR (2)      NOT NULL,
    [DI_CNC_TAX_OPI]     BIT           NOT NULL,
    [DC_TAX_OPI_SRC]     CHAR (2)      NOT NULL,
    PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);



