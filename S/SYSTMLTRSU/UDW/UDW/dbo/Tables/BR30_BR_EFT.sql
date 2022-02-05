CREATE TABLE [dbo].[BR30_BR_EFT] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [BN_EFT_SEQ]         SMALLINT        NOT NULL,
    [BF_EFT_ABA]         VARBINARY (128) NOT NULL,
    [BF_EFT_ACC]         VARBINARY (128) NOT NULL,
    [BC_EFT_TYP_ACC]     CHAR (1)        NOT NULL,
    [BC_EFT_STA]         CHAR (1)        NOT NULL,
    [BD_EFT_STA]         DATE            NULL,
    [BF_LST_DTS_BR30]    DATETIME2 (7)   NOT NULL,
    [BD_EFT_PNO_SNT]     DATE            NULL,
    [BA_EFT_ADD_WDR]     NUMERIC (7, 2)  NOT NULL,
    [BN_EFT_NSF_CTR]     NUMERIC (1)     NOT NULL,
    [BN_EFT_DAY_DUE]     VARCHAR (2)     NOT NULL,
    [BA_EFT_LST_WDR]     NUMERIC (7, 2)  NOT NULL,
    [BA_EFT_TOL]         NUMERIC (5, 2)  NOT NULL,
    [BC_EFT_DNL_REA]     CHAR (1)        NOT NULL,
    [DF_PRS_ID]          CHAR (9)        NOT NULL,
    [BC_EFT_PAY_OPT]     CHAR (1)        NOT NULL,
    [BC_SRC_DIR_DBT_APL] CHAR (1)        NOT NULL,
    [BC_DDT_PAY_PRS_TYP] CHAR (1)        NOT NULL,
    [BF_DDT_PAY_EDS]     VARCHAR (10)    NOT NULL,
    CONSTRAINT [PK_BR30_BR_EFT] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [BN_EFT_SEQ] ASC)
);

