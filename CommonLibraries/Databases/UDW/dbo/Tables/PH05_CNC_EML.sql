﻿CREATE TABLE [dbo].[PH05_CNC_EML] (
    [DF_CNC_SYS_ID]      CHAR (3)      NOT NULL,
    [DF_SPE_ID]          CHAR (10)     NOT NULL,
    [DX_CNC_EML_ADR]     VARCHAR (254) NOT NULL,
    [DC_EML_APL_SYS_SRC] CHAR (1)      NOT NULL,
    [DC_CNC_EML_DAT_SRC] VARCHAR (2)   NOT NULL,
    [DF_DTS_EML_ADR_EFF] DATETIME      NULL,
    [DF_LST_USR_EML_ADR] VARCHAR (8)   NOT NULL,
    [DI_VLD_CNC_EML_ADR] CHAR (1)      NOT NULL,
    [DC_VLD_EML_APL_SRC] CHAR (1)      NOT NULL,
    [DC_VLD_CNC_EML_SRC] VARCHAR (2)   NOT NULL,
    [DF_DTS_VLD_EML_EFF] DATETIME      NULL,
    [DF_LST_USR_VLD_EML] VARCHAR (8)   NOT NULL,
    [DI_CNC_ELT_OPI]     CHAR (1)      NOT NULL,
    [DC_ELT_OPI_APL_SRC] CHAR (1)      NOT NULL,
    [DC_ELT_OPI_SRC]     VARCHAR (2)   NOT NULL,
    [DF_DTS_ELT_OPI_EFF] DATETIME      NULL,
    [DF_LST_USR_ELT_OPI] VARCHAR (8)   NOT NULL,
    [DI_CNC_EBL_OPI]     CHAR (1)      NOT NULL,
    [DC_EBL_OPI_APL_SRC] CHAR (1)      NOT NULL,
    [DC_EBL_OPI_SRC]     VARCHAR (2)   NOT NULL,
    [DF_DTS_EBL_OPI_EFF] DATETIME      NULL,
    [DF_LST_USR_EBL_OPI] VARCHAR (8)   NOT NULL,
    [DF_LST_DTS_PH05]    DATETIME      NOT NULL,
    [DF_LST_USR_PH05]    VARCHAR (8)   NOT NULL,
    [DD_CNC_EML_ADR_VER] DATETIME      NULL,
    [DF_LST_DTS_EML_VER] DATETIME      NULL,
    [DC_LST_EML_VER_SYS] CHAR (1)      NOT NULL,
    [DC_LST_EML_VER_SRC] VARCHAR (2)   NOT NULL,
    [DX_CNC_EML_ADR_UC]  VARCHAR (254) NOT NULL,
    [DF_LST_USR_EML_VER] CHAR (8)      NOT NULL,
    [DI_CNC_TAX_OPI]     CHAR (1)      NOT NULL,
    [DC_TAX_OPI_APL_SRC] CHAR (1)      NOT NULL,
    [DC_TAX_OPI_SRC]     VARCHAR (2)   NOT NULL,
    [DF_DTS_TAX_OPI_EFF] DATETIME      NULL,
    [DF_LST_USR_TAX_OPI] VARCHAR (8)   NOT NULL,
    [DF_ACP_TRM_VRS_ID]  VARCHAR (7)   NOT NULL,
    [DF_DTS_ACP_TRM_EFF] DATETIME      NULL,
    [DF_LST_USR_ACP_TRM] VARCHAR (8)   NOT NULL,
    CONSTRAINT [PK_PH05_CNC_EML] PRIMARY KEY CLUSTERED ([DF_CNC_SYS_ID] ASC, [DF_SPE_ID] ASC) WITH (FILLFACTOR = 95)
);


GO
CREATE NONCLUSTERED INDEX [CIX_DF_SPE_ID]
    ON [dbo].[PH05_CNC_EML]([DF_SPE_ID] ASC)
    INCLUDE([DX_CNC_EML_ADR], [DI_VLD_CNC_EML_ADR], [DI_CNC_ELT_OPI]) WITH (FILLFACTOR = 95);

