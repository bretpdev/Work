CREATE TABLE [dbo].[RS05_IBR_RPS] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [BD_CRT_RS05]        DATE            NOT NULL,
    [BN_IBR_SEQ]         SMALLINT        NOT NULL,
    [BF_CRT_USR_RS05]    CHAR (8)        NOT NULL,
    [BF_CRY_YR]          CHAR (4)        NOT NULL,
    [BC_ST_IBR]          CHAR (2)        NOT NULL,
    [BC_STA_RS05]        CHAR (1)        NOT NULL,
    [BA_AGI]             NUMERIC (12, 2) NOT NULL,
    [BN_MEM_HSE_HLD]     NUMERIC (2)     NOT NULL,
    [BA_PMN_STD_TOT_PAY] NUMERIC (12, 2) NOT NULL,
    [BC_IBR_INF_SRC_VER] CHAR (3)        NOT NULL,
    [BF_LST_DTS_RS05]    DATETIME2 (7)   NOT NULL,
    [BF_SSN_SPO]         CHAR (9)        NULL,
    [BC_IRS_TAX_FIL_STA] CHAR (1)        NOT NULL,
    [BI_JNT_BR_SPO_RPY]  CHAR (1)        NOT NULL,
    [BD_ANV_QLF_IBR]     DATE            NULL,
    [BC_DOC_SNT_BR_IDR]  CHAR (1)        NOT NULL,
    [BC_BR_REQ_PLN_RPYE] CHAR (1)        NOT NULL,
    [BC_BR_GDE_LVL_RPYE] CHAR (1)        NOT NULL,
    [BI_REQ_DFR_FOR_CUT] CHAR (1)        NOT NULL,
    [BC_DFR_FOR_CUT_TYP] CHAR (1)        NOT NULL,
    [LA_RPYE_CTH_RPS]    NUMERIC (12, 2) NULL,
    [BC_RPA_CTH_DAT_USE] CHAR (1)        NOT NULL,
    [BD_RPD_SCH_TYP]     DATE            NULL,
    [AF_IDR_APL_ID]      CHAR (10)       NULL,
    CONSTRAINT [PK_RS05_IBR_RPS] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [BD_CRT_RS05] ASC, [BN_IBR_SEQ] ASC) WITH (FILLFACTOR = 95)
);



