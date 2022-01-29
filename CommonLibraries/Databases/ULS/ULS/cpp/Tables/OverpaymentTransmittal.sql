CREATE TABLE [cpp].[OverpaymentTransmittal] (
    [OverpaymentTransmittalsId] INT          IDENTITY (1, 1) NOT NULL,
    [AccountNumber]             CHAR (10)    NOT NULL,
    [LoanSequence]              INT          NOT NULL,
    [FirstDisbursementDate]     DATETIME     NOT NULL,
    [ManifestNumber]            CHAR (15)    NOT NULL,
    [LoanType]                  CHAR (1)     NOT NULL,
    [OriginatorLoanId]          CHAR (13)    NOT NULL,
    [NsldsId]                   CHAR (17)    NOT NULL,
    [Active]                    BIT          DEFAULT ((1)) NOT NULL,
    [CreatedAt]                 DATETIME     CONSTRAINT [DF_OverpaymentTransmittals_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                 VARCHAR (25) CONSTRAINT [DF_OverpaymentTransmittals_CreatedBy] DEFAULT (suser_sname()) NOT NULL,
    PRIMARY KEY CLUSTERED ([OverpaymentTransmittalsId] ASC) WITH (FILLFACTOR = 95)
);

