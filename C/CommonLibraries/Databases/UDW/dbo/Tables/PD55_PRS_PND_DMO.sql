﻿CREATE TABLE [dbo].[PD55_PRS_PND_DMO] (
    [DF_PRS_ID]          VARCHAR (9)   NOT NULL,
    [DN_PDM_PND_SEQ]     SMALLINT      NOT NULL,
    [DC_3PT_PDM_PND]     VARCHAR (2)   NOT NULL,
    [DD_BRT_PND]         DATE          NULL,
    [DM_PRS_SFX_PRV_PND] VARCHAR (4)   NOT NULL,
    [DM_PRS_LST_PRV_PND] VARCHAR (23)  NOT NULL,
    [DM_PRS_MID_PRV_PND] VARCHAR (13)  NOT NULL,
    [DM_PRS_1_PRV_PND]   VARCHAR (13)  NOT NULL,
    [DM_PRS_LST_SFX_PND] VARCHAR (4)   NOT NULL,
    [DM_PRS_LST_PND]     VARCHAR (23)  NOT NULL,
    [DM_PRS_MID_PND]     VARCHAR (13)  NOT NULL,
    [DM_PRS_1_PND]       VARCHAR (13)  NOT NULL,
    [DF_3PT_PDM_PND]     VARCHAR (9)   NOT NULL,
    [DC_PDM_SRC_PND]     VARCHAR (2)   NOT NULL,
    [DD_PHN_LST_VER_PND] DATE          NULL,
    [DN_FGN_PHN_INL_PND] VARCHAR (3)   NOT NULL,
    [DN_FGN_PHN_CNY_PND] VARCHAR (3)   NOT NULL,
    [DN_FGN_PHN_LCL_PND] VARCHAR (11)  NOT NULL,
    [DN_FGN_PHN_CT_PND]  VARCHAR (5)   NOT NULL,
    [DN_FGN_PHN_XTN_PND] VARCHAR (5)   NOT NULL,
    [DT_PHN_BCL_PND]     TIME (0)      NULL,
    [DN_DOM_PHN_ARA_PND] VARCHAR (3)   NOT NULL,
    [DN_DOM_PHN_XCH_PND] VARCHAR (3)   NOT NULL,
    [DN_DOM_PHN_LCL_PND] VARCHAR (4)   NOT NULL,
    [DN_DOM_PHN_XTN_PND] VARCHAR (5)   NOT NULL,
    [DM_FGN_CNY_PND]     VARCHAR (25)  NOT NULL,
    [DF_ZIP_CDE_PND]     VARCHAR (17)  NOT NULL,
    [DM_FGN_ST_PND]      VARCHAR (15)  NOT NULL,
    [DF_LST_DTS_PD55]    DATETIME2 (7) NOT NULL,
    [DC_DOM_ST_PND]      VARCHAR (2)   NOT NULL,
    [DX_STR_ADR_1_PND]   VARCHAR (30)  NOT NULL,
    [DX_STR_ADR_2_PND]   VARCHAR (30)  NOT NULL,
    [DX_STR_ADR_3_PND]   VARCHAR (30)  NOT NULL,
    [DM_CT_PND]          VARCHAR (20)  NOT NULL,
    [DC_ST_DRV_LIC_PND]  VARCHAR (2)   NOT NULL,
    [DF_DRV_LIC_PND]     VARCHAR (20)  NOT NULL,
    [DD_VER_ADR_PND]     DATE          NULL,
    [DD_NME_VER_LST_PND] DATE          NULL,
    [WF_QUE]             VARCHAR (2)   NOT NULL,
    [WF_SUB_QUE]         VARCHAR (2)   NOT NULL,
    [DC_FGN_CNY_PND]     VARCHAR (2)   NOT NULL,
    [DC_PHN_SRC_PND]     VARCHAR (2)   NOT NULL,
    [DC_ADR_EML_PND]     CHAR (1)      NOT NULL,
    [DC_SRC_ADR_EML_PND] VARCHAR (2)   NOT NULL,
    [DC_SEX_PND]         CHAR (1)      NOT NULL,
    [DF_ALN_RGS_PND]     VARCHAR (9)   NOT NULL,
    [DI_US_CTZ_PND]      CHAR (1)      NOT NULL,
    [DD_DRV_LIC_REN_PND] DATE          NULL,
    [DC_ADR_PND]         CHAR (1)      NOT NULL,
    [DC_PHN_PND]         CHAR (1)      NOT NULL,
    [DX_ADR_EML_PND]     VARCHAR (254) NULL,
    [DD_EML_LST_VER_PND] DATE          NULL,
    [DC_ALW_ADL_PHN_PND] CHAR (1)      NOT NULL,
    CONSTRAINT [PK_PD55_PRS_PND_DMO] PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [DN_PDM_PND_SEQ] ASC) WITH (FILLFACTOR = 95)
);
