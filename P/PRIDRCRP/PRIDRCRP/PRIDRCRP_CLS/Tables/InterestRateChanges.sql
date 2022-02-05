CREATE TABLE [pridrcrp].[InterestRateChanges] (
    [InterestRateChangeId] INT             IDENTITY (1, 1) NOT NULL,
    [BorrowerInformationId] INT            NOT NULL,
    [BorrowerActivityId]   INT             NOT NULL,
    [InterestRate]         DECIMAL (14, 3) NULL,
    [EffectiveDate]        DATE            NULL,
    [InactivatedAt]        DATE            NULL,
    [InactivatedBy]        VARCHAR (200)   NULL,
    PRIMARY KEY CLUSTERED ([InterestRateChangeId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_BorrowerActivityHistory_InterestRateChanges] FOREIGN KEY ([BorrowerActivityId]) REFERENCES [pridrcrp].[BorrowerActivityHistory] ([BorrowerActivityId]),
    CONSTRAINT [FK_BorrowerInformation_InterestRateChanges] FOREIGN KEY ([BorrowerInformationId]) REFERENCES [pridrcrp].[BorrowerInformation] ([BorrowerInformationId])
);
GO

CREATE NONCLUSTERED INDEX [ix_InterestRateChanges_BorrowerInformationId_BorrowerActivityId_InterestRate_EffectiveDate]
    ON [pridrcrp].[InterestRateChanges]([BorrowerInformationId] ASC, [BorrowerActivityId] ASC, [InterestRate] ASC, [EffectiveDate] ASC) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);
GO

