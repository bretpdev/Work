﻿CREATE TABLE [dbo].[FB10_BR_FOR_REQ] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [LF_FOR_CTL_NUM]     VARCHAR (3)     NOT NULL,
    [LC_FOR_TYP]         VARCHAR (2)     NOT NULL,
    [LC_FOR_SUB_TYP]     CHAR (1)        NOT NULL,
    [LI_FOR_BR_SIG]      CHAR (1)        NOT NULL,
    [LD_FOR_REQ_BEG]     DATE            NOT NULL,
    [LC_FOR_REQ_COR]     VARCHAR (3)     NOT NULL,
    [LD_FOR_REQ_END]     DATE            NOT NULL,
    [LF_USR_CRT_REQ_FOR] VARCHAR (8)     NOT NULL,
    [LD_CRT_REQ_FOR]     DATE            NOT NULL,
    [LI_CAP_FOR_INT_REQ] CHAR (1)        NOT NULL,
    [LC_FOR_STA]         CHAR (1)        NOT NULL,
    [LD_STA_FOR10]       DATE            NOT NULL,
    [LC_STA_FOR10]       CHAR (1)        NOT NULL,
    [LI_PNR_SIG]         CHAR (1)        NOT NULL,
    [LI_XED_ATH]         CHAR (1)        NOT NULL,
    [LI_FOR_COS_SIG]     CHAR (1)        NOT NULL,
    [LI_FOR_RES_NG]      CHAR (1)        NOT NULL,
    [LF_LST_DTS_FB10]    DATETIME2 (7)   NOT NULL,
    [LN_RPS_SEQ]         SMALLINT        NULL,
    [LF_FOR_PRV]         VARCHAR (3)     NULL,
    [LI_FOR_CER_OCL_SIG] CHAR (1)        NOT NULL,
    [LI_FOR_DOD_FRM_SPY] CHAR (1)        NOT NULL,
    [LI_FOR_BR_ENR_VER]  CHAR (1)        NOT NULL,
    [LI_FOR_BR_EMP_FT]   CHAR (1)        NOT NULL,
    [LI_FOR_BR_POO_INC]  CHAR (1)        NOT NULL,
    [LI_FOR_BR_FED_IXR]  CHAR (1)        NOT NULL,
    [LI_FOR_BR_MIN_INC]  CHAR (1)        NOT NULL,
    [LI_FOR_BR_POO_PAY]  CHAR (1)        NOT NULL,
    [LD_FOR_INF_CER]     DATE            NULL,
    [LI_CMK_ELG_FOR]     CHAR (1)        NOT NULL,
    [LF_DOE_SCL_FOR]     VARCHAR (8)     NOT NULL,
    [LA_REQ_RDC_PAY]     NUMERIC (7, 2)  NULL,
    [LI_FOR_MED_IRN]     CHAR (1)        NOT NULL,
    [LI_FOR_MED_LIC_CER] CHAR (1)        NOT NULL,
    [LC_FOR_XCP_DCR_TYP] VARCHAR (2)     NOT NULL,
    [LC_FOR_NEW_SUB_TYP] VARCHAR (2)     NOT NULL,
    [LI_FOR_COV_DLQ]     CHAR (1)        NOT NULL,
    [LA_BR_PAY_CHK_JOB]  NUMERIC (12, 2) NULL,
    [LC_BR_PAY_CHK_FRQ]  VARCHAR (2)     NOT NULL,
    [LD_FOR_BR_REQ_BEG]  DATE            NULL,
    [LD_FOR_BR_REQ_END]  DATE            NULL,
    [LC_FOR_DNL_USR_ENT] VARCHAR (3)     NOT NULL,
    [LI_FOR_SPT_DOC_ACP] CHAR (1)        NOT NULL,
    [LA_BR_MTH_IRL_ISL]  NUMERIC (12, 2) NULL,
    [LA_BR_MTH_EXT_ISL]  NUMERIC (12, 2) NULL,
    [LI_BRQ_TMP_DNL_FOR] CHAR (1)        NOT NULL,
    [LI_BRQ_TMP_FOR_DLQ] CHAR (1)        NOT NULL,
    [PI_FOR_CAP_DSG]     VARCHAR (1)     NULL
);




GO
CREATE NONCLUSTERED INDEX [IX_BFSSN_LFFORCTLNUM_LCFORSTA_LCSTAFOR10]
    ON [dbo].[FB10_BR_FOR_REQ]([BF_SSN] ASC, [LF_FOR_CTL_NUM] ASC, [LC_FOR_STA] ASC, [LC_STA_FOR10] ASC)
    INCLUDE([LC_FOR_TYP]) WITH (FILLFACTOR = 95);

