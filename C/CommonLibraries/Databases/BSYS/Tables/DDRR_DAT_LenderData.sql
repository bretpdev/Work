CREATE TABLE [dbo].[DDRR_DAT_LenderData] (
    [ID]                 VARCHAR (50) NOT NULL,
    [ProcessDate]        DATETIME     NOT NULL,
    [LoanProgram]        VARCHAR (10) NOT NULL,
    [DisbursementMethod] VARCHAR (50) NOT NULL,
    [BorrowerCount]      INT          NOT NULL,
    [LoanCount]          INT          NOT NULL,
    [GrossDisbursement]  MONEY        NOT NULL,
    [NetDisbursement]    MONEY        NOT NULL,
    [FundingAmount]      MONEY        NOT NULL,
    [OFee]               MONEY        NOT NULL,
    [GFee]               MONEY        NOT NULL,
    CONSTRAINT [PK_DDRR_DAT_LenderData] PRIMARY KEY CLUSTERED ([ID] ASC, [ProcessDate] ASC, [LoanProgram] ASC, [DisbursementMethod] ASC)
);

