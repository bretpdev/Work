CREATE TABLE [pridrcrp].[Disbursements] (
    [DisbursementId]       INT             IDENTITY (1, 1) NOT NULL,
    [BorrowerInformationId] INT             NOT NULL,
    [DisbursementDate]     DATE            NOT NULL,
    [InterestRate]         DECIMAL (14, 3) NOT NULL,
    [LoanType]             VARCHAR (50)    NOT NULL,
    [DisbursementNumber]   VARCHAR (2)     NOT NULL,
    [LoanId]               VARCHAR (15)    NOT NULL,
    [DisbursementAmount]   DECIMAL (14, 2) NOT NULL,
    [CapInterest]          DECIMAL (14, 2) NOT NULL,
    [RefundCancel]         DECIMAL (14, 2) NOT NULL,
    [BorrPaidPrin]         DECIMAL (14, 2) NOT NULL,
    [PrinOutstanding]      DECIMAL (14, 2) NOT NULL,
    [PaidInterest]         DECIMAL (14, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([DisbursementId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_BorrowerInformation_Disbursements] FOREIGN KEY ([BorrowerInformationId]) REFERENCES [pridrcrp].[BorrowerInformation] ([BorrowerInformationId])
);
GO

CREATE NONCLUSTERED INDEX [IX_Disbursements_BorrowerInformationId]
    ON [pridrcrp].[Disbursements]([BorrowerInformationId] ASC, [DisbursementDate] ASC, [InterestRate] ASC, [LoanType] ASC, [DisbursementNumber] ASC, [LoanId] ASC, [DisbursementAmount] ASC, [CapInterest] ASC, [RefundCancel] ASC, [BorrPaidPrin] ASC, [PrinOutstanding] ASC, [PaidInterest] ASC)
    INCLUDE([DisbursementId]) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);
GO

