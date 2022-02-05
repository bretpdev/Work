CREATE TABLE [dbo].[RF10_RFR] (
    [BF_SSN]             VARCHAR (9) NOT NULL,
    [BN_SEQ_RFR]         SMALLINT    NOT NULL,
    [BC_STA_REFR10]      CHAR (1)    NULL,
    [BI_ATH_3_PTY]       CHAR (1)    NULL,
    [BC_RFR_REL_BR]      VARCHAR (2) NULL,
    [BD_EFF_RFR]         DATE        NULL,
    [BF_RFR]             VARCHAR (9) NULL,
    [BC_RFR_TYP]         CHAR (1)    NULL,
    [BF_LST_DTS_RF10]    DATETIME    NULL,
    [BD_EFF_RFR_HST]     DATE        NULL,
    [BC_REA_RFR_HST]     CHAR (1)    NULL,
    [BF_LST_USR_HST_RFR] VARCHAR (8) NULL,
    [BD_ATH_3_PTY_END]   DATE        NULL,
    PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [BN_SEQ_RFR] ASC)
);

