CREATE TABLE [pridrcrp].[PaymentHistory] (
    [TransactionId]        INT             IDENTITY (1, 1) NOT NULL,
    [BorrowerInformationId] INT            NOT NULL,
    [Description]          VARCHAR (20)    NOT NULL,
    [ActionDate]           DATE            NULL,
    [EffectiveDate]        DATE            NULL,
    [TotalPaid]            DECIMAL (14, 2) NULL,
    [InterestPaid]         DECIMAL (14, 2) NULL,
    [PrincipalPaid]        DECIMAL (14, 2) NULL,
    [LcNsfPaid]            DECIMAL (14, 2) NULL,
    [AccruedInterest]      DECIMAL (14, 2) NULL,
    [LcNsfDue]             DECIMAL (14, 2) NULL,
    [PrincipalBalance]     DECIMAL (14, 2) NULL,
	[PaymentSatisfactionProcessed] BIT DEFAULT(0) NOT NULL,
    PRIMARY KEY CLUSTERED ([TransactionId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_BorrowerInformation_PaymentHistory] FOREIGN KEY ([BorrowerInformationId]) REFERENCES [pridrcrp].[BorrowerInformation] ([BorrowerInformationId])
);
GO

CREATE NONCLUSTERED INDEX [IX_PaymentHistory_BorrowerInformationId_includes]
    ON [pridrcrp].[PaymentHistory]([BorrowerInformationId] ASC)
    INCLUDE([TransactionId], [Description], [ActionDate], [EffectiveDate], [TotalPaid], [InterestPaid], [PrincipalPaid], [LcNsfPaid], [AccruedInterest], [LcNsfDue], [PrincipalBalance]) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);
GO

