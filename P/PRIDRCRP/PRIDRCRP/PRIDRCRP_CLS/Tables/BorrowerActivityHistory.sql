CREATE TABLE [pridrcrp].[BorrowerActivityHistory] (
    [BorrowerActivityId]   INT           IDENTITY (1, 1) NOT NULL,
    [BorrowerInformationId] INT           NOT NULL,
    [ActivityDate]         DATE          NOT NULL,
    [ActivityDescription]  VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([BorrowerActivityId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_BorrowerInformation_BwrAtyHst] FOREIGN KEY ([BorrowerInformationId]) REFERENCES [pridrcrp].[BorrowerInformation] ([BorrowerInformationId])
);
GO

CREATE NONCLUSTERED INDEX [IX_BorrowerActivityHistory_BorrowerInformationId_includes]
    ON [pridrcrp].[BorrowerActivityHistory]([BorrowerInformationId] ASC)
    INCLUDE([BorrowerActivityId], [ActivityDate], [ActivityDescription]) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);
GO

