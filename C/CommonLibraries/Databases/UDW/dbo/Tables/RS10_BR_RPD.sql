CREATE TABLE [dbo].[RS10_BR_RPD] (
    [BF_SSN]             CHAR (9)      NOT NULL,
    [LN_RPS_SEQ]         SMALLINT      NOT NULL,
    [LD_STA_RPST10]      DATE          NOT NULL,
    [LC_STA_RPST10]      CHAR (1)      NOT NULL,
    [LC_FRQ_PAY]         CHAR (1)      NOT NULL,
    [LI_SIG_RPD_DIS]     CHAR (1)      NOT NULL,
    [LD_RPS_1_PAY_DU]    DATE          NULL,
    [LC_RPD_DIS]         CHAR (1)      NOT NULL,
    [LD_SNT_RPD_DIS]     DATE          NULL,
    [LD_RTN_RPD_DIS]     DATE          NULL,
    [LF_LST_DTS_RS10]    DATETIME2 (7) NOT NULL,
    [LC_RPS_OPT_PRT]     CHAR (1)      NOT NULL,
    [LF_USR_RPS_REQ]     VARCHAR (8)   NOT NULL,
    [LN_BR_REQ_DU_DAY]   VARCHAR (2)   NOT NULL,
    [BD_CRT_RS05]        DATE          NULL,
    [BN_IBR_SEQ]         SMALLINT      NULL,
    [LC_RPY_FIX_TRM_AMT] CHAR (1)      NOT NULL,
    [LC_CAP_TRG_LVE_PFH] VARCHAR (2)   NOT NULL,
    CONSTRAINT [PK_RS10_BR_RPD] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_RPS_SEQ] ASC) WITH (FILLFACTOR = 95)
);

