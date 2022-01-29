CREATE TABLE [pridrcrp].[PaymentAmountChanges] (
    [PaymentAmountChangeId] INT             IDENTITY (1, 1) NOT NULL,
    [BorrowerInformationId]  INT            NOT NULL,
    [BorrowerActivityId]    INT             NOT NULL,
    [PaymentAmount]         DECIMAL (14, 2) NULL,
    [EffectiveDate]         DATE            NULL,
    [InactivatedAt]         DATE            NULL,
    [InactivatedBy]         VARCHAR (200)   NULL,
    PRIMARY KEY CLUSTERED ([PaymentAmountChangeId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_BorrowerActivityHistory_PaymentAmountChanges] FOREIGN KEY ([BorrowerActivityId]) REFERENCES [pridrcrp].[BorrowerActivityHistory] ([BorrowerActivityId]),
    CONSTRAINT [FK_BorrowerInformation_PaymentAmountChanges] FOREIGN KEY ([BorrowerInformationId]) REFERENCES [pridrcrp].[BorrowerInformation] ([BorrowerInformationId])
);
GO

CREATE NONCLUSTERED INDEX [IX_PaymentAmountChanges_BorrowerInformationId_BorrowerActivityId_PaymentAmount_EffectiveDate]
    ON [pridrcrp].[PaymentAmountChanges]([BorrowerInformationId] ASC, [BorrowerActivityId] ASC, [PaymentAmount] ASC, [EffectiveDate] ASC) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);
GO

