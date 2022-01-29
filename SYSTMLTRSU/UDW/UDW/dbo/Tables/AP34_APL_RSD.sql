﻿CREATE TABLE [dbo].[AP34_APL_RSD] (
    [AF_APL_ID]          VARCHAR (9)   NOT NULL,
    [AC_APL_PRS_TYP]     CHAR (1)      NOT NULL,
    [DF_PRS_ID]          VARCHAR (9)   NOT NULL,
    [AI_PRV_DFL]         CHAR (1)      NOT NULL,
    [AC_CTZ]             CHAR (1)      NOT NULL,
    [AF_ALN_RGS]         VARCHAR (10)  NOT NULL,
    [AC_ST_RSD]          VARCHAR (2)   NOT NULL,
    [AD_ST_RSD]          DATE          NULL,
    [AN_YR_CUR_ADR]      NUMERIC (2)   NULL,
    [AN_MTH_CUR_ADR]     NUMERIC (2)   NULL,
    [AN_YR_PRV_ADR]      NUMERIC (2)   NULL,
    [AN_MTH_PRV_ADR]     NUMERIC (2)   NULL,
    [AF_LST_USR_AP34]    VARCHAR (8)   NOT NULL,
    [AF_LST_DTS_AP34]    DATETIME2 (7) NOT NULL,
    [AC_REL_TO_WHO]      CHAR (1)      NOT NULL,
    [AF_REL_PRS_ID]      VARCHAR (9)   NOT NULL,
    [AC_REL_TYP]         VARCHAR (2)   NOT NULL,
    [AI_PAA_RVW]         CHAR (1)      NOT NULL,
    [AI_CER_EDU_XPE_RVW] CHAR (1)      NOT NULL,
    [AI_COR_OPT_OUT]     CHAR (1)      NOT NULL,
    [AI_MIN_US_RSD_RQR]  CHAR (1)      NOT NULL,
    [AI_ALN_RGS_DOC_RCV] CHAR (1)      NOT NULL,
    [AC_ALT_PRS_ELG_STA] VARCHAR (2)   NOT NULL,
    [AC_ALT_PRS_ACT_STA] CHAR (1)      NOT NULL,
    [AC_APL_SLC_STA]     CHAR (1)      NOT NULL,
    [AF_APL_SLC_STA_DTS] DATETIME2 (7) NULL,
    [AC_PIO_ESG_LRT]     CHAR (1)      NOT NULL,
    [AF_CRD_DIS_VRS]     VARCHAR (9)   NOT NULL,
    [AD_CRD_DIS_EVL_END] DATE          NULL,
    [AC_PRS_PIO_ELG_STA] VARCHAR (2)   NOT NULL,
    [AC_PRS_REM_APL_REA] CHAR (1)      NOT NULL,
    [AC_CUR_ESG_LRT]     CHAR (1)      NOT NULL,
    CONSTRAINT [PK_AP34_APL_RSD] PRIMARY KEY CLUSTERED ([AF_APL_ID] ASC, [AC_APL_PRS_TYP] ASC, [DF_PRS_ID] ASC) WITH (FILLFACTOR = 95)
);

