﻿CREATE TABLE [dbo].[FS10_DL_LON] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [LN_SEQ]             SMALLINT        NOT NULL,
    [LF_DL_MPN]          VARCHAR (21)    NOT NULL,
    [LC_DL_STA_MPN]      CHAR (1)        NOT NULL,
    [LI_DL_ELC_MPN]      CHAR (1)        NOT NULL,
    [LF_DL_FIN_AWD_YR]   VARCHAR (4)     NOT NULL,
    [LD_DL_STU_ACA_BEG]  DATE            NULL,
    [LD_DL_STU_ACA_END]  DATE            NULL,
    [LC_DL_STU_DEP_STA]  CHAR (1)        NOT NULL,
    [LR_DL_DSB_REB_PER]  NUMERIC (6, 3)  NULL,
    [LI_DL_BR_ELG_HPA]   CHAR (1)        NOT NULL,
    [LI_DL_XED_USB_LMT]  CHAR (1)        NOT NULL,
    [LF_LST_USR_FS10]    VARCHAR (8)     NOT NULL,
    [LF_LST_DTS_FS10]    DATETIME2 (7)   NOT NULL,
    [LD_LON_ICR_ENT]     DATE            NULL,
    [LC_LON_ICR_RPY_TYP] CHAR (1)        NOT NULL,
    [LF_FED_AWD]         VARCHAR (18)    NOT NULL,
    [LN_FED_AWD_SEQ]     NUMERIC (3)     NULL,
    [LC_EFT_BBS_FED]     VARCHAR (3)     NOT NULL,
    [LD_ITL_LN_SLD_DOE]  DATE            NULL,
    [LD_DL_BR_SSN_CHG]   DATE            NULL,
    [LF_ORG_DL_BR_SSN]   CHAR (9)        NOT NULL,
    [LD_ORG_DL_BR_DOB]   DATE            NULL,
    [LF_ORG_DL_STU_SSN]  CHAR (9)        NOT NULL,
    [LD_ORG_DL_STU_DOB]  DATE            NULL,
    [LF_ORG_DL_EDS_SSN]  CHAR (9)        NOT NULL,
    [LF_ORG_DL_LON]      VARCHAR (2)     NOT NULL,
    [LI_FRC_ICR]         CHAR (1)        NOT NULL,
    [LD_DL_CON_INQ]      DATE            NULL,
    [LC_CON_APL_SCH_TYP] VARCHAR (2)     NOT NULL,
    [LA_ICR_CAP_LMT_NEG] NUMERIC (12, 2) NULL,
    [LA_ICR_BAL_RPY_SR]  NUMERIC (12, 2) NULL,
    [LA_ICR_NEG_AMR_IC]  NUMERIC (12, 2) NULL,
    [LI_AUT_COD_CON_PAY] CHAR (1)        NOT NULL,
    [LD_AUT_COD_CON_REV] DATE            NULL,
    [LN_PRE_06_RPD_TRM]  NUMERIC (3)     NULL,
    [LA_COD_FIN_AWD]     NUMERIC (12, 2) NULL,
    [LF_FED_LON_PR_GRP]  VARCHAR (10)    NOT NULL,
    [LC_DL_PRV_RPY_PLN]  CHAR (2)        NOT NULL,
    [LD_ENT_ICR_PAYE]    DATE            NULL,
    [LA_PAE_CAP_LMT_NEG] NUMERIC (12, 2) NULL,
    [LA_PAE_LON_BAL_SR]  NUMERIC (12, 2) NULL,
    [LA_PAE_NEG_AMR_IC]  NUMERIC (12, 2) NULL,
    [LF_CON_LON_ORG]     VARCHAR (6)     NOT NULL,
    [LI_FRC_PAYE]        CHAR (1)        NOT NULL,
    [LI_FRC_RPYE]        CHAR (1)        NOT NULL,
    [LD_LON_ENT_RPYE]    DATE            NULL,
    [LA_RPYE_CTH_RMN]    NUMERIC (12, 2) NULL,
    [LD_COD_0_AWD_AMT]   DATE            NULL,
    CONSTRAINT [PK_FS10_DL_LON] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC) WITH (FILLFACTOR = 95)
);

