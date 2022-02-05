CREATE TABLE [dbo].[LN65_RepaymentSched] (
    [DF_SPE_ACC_ID]  VARCHAR (10)   NOT NULL,
    [LN_SEQ]         INT            NOT NULL,
    [LD_CRT_LON65]   VARCHAR (10)   CONSTRAINT [DF_RPS_LD_CRT_LON65] DEFAULT (' ') NULL,
    [TYP_SCH_DIS]    VARCHAR (40)   CONSTRAINT [DF_RPS_TYP_SCH_DIS] DEFAULT (' ') NULL,
    [LD_SNT_RPD_DIS] VARCHAR (10)   CONSTRAINT [DF_RPS_LD_SNT_RPD_DIS] DEFAULT (' ') NULL,
    [LN_RPS_TRM]     INT            CONSTRAINT [DF_RPS_LN_RPS_TRM] DEFAULT ((0)) NULL,
    [DUE_DAY]        VARCHAR (2)    CONSTRAINT [DF_RPS_DUE_DAY] DEFAULT ('') NULL,
    [LA_RPS_ISL]     NUMERIC (9, 2) CONSTRAINT [DF_RPS_LA_RPS_ISL] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_RPS] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Installment amount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN65_RepaymentSched', @level2type = N'COLUMN', @level2name = N'LA_RPS_ISL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Day of month payments are due', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN65_RepaymentSched', @level2type = N'COLUMN', @level2name = N'DUE_DAY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Repayment term', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN65_RepaymentSched', @level2type = N'COLUMN', @level2name = N'LN_RPS_TRM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Repayment disclosure sent date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN65_RepaymentSched', @level2type = N'COLUMN', @level2name = N'LD_SNT_RPD_DIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Repayment disclosure type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN65_RepaymentSched', @level2type = N'COLUMN', @level2name = N'TYP_SCH_DIS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Repayment schedule create date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN65_RepaymentSched', @level2type = N'COLUMN', @level2name = N'LD_CRT_LON65';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN65_RepaymentSched', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN65_RepaymentSched', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';

