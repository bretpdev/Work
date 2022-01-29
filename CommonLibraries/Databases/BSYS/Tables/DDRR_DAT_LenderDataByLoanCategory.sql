CREATE TABLE [dbo].[DDRR_DAT_LenderDataByLoanCategory] (
    [ID]                      VARCHAR (50) NOT NULL,
    [DisbursementDate]        DATETIME     NOT NULL,
    [LoanCategory]            VARCHAR (50) NOT NULL,
    [GrossDisbursementAmount] MONEY        NOT NULL,
    [BorrowerOriginationFee]  MONEY        NOT NULL,
    [LenderOriginationFee]    MONEY        NOT NULL,
    [UHEAAOriginationFee]     MONEY        NOT NULL,
    [GuaranteeFee]            MONEY        NOT NULL,
    [FundingAmount]           MONEY        NOT NULL,
    CONSTRAINT [PK_DDRR_DAT_LenderDataByLoanCategory] PRIMARY KEY CLUSTERED ([ID] ASC, [DisbursementDate] ASC, [LoanCategory] ASC)
);

