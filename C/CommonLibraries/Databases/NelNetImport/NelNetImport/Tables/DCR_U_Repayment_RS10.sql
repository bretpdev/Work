CREATE TABLE [dbo].[DCR_U_Repayment_RS10] (
    [nn_ln_seq]       INT         NOT NULL,
    [BF_SSN]          VARCHAR (9) NULL,
    [LN_SEQ]          INT         NULL,
    [LN_RPS_SEQ_temp] INT         NOT NULL,
    [LC_STA_RPST10]   VARCHAR (1) NOT NULL,
    [LC_FRQ_PAY]      VARCHAR (1) NOT NULL,
    [LI_SIG_RPD_DIS]  VARCHAR (1) NOT NULL,
    [LD_RPS_1_PAY_DU] DATE        NULL,
    [LC_RPD_DIS]      INT         NULL,
    [LD_SNT_RPD_DIS]  DATE        NULL
);

