CREATE TABLE [dbo].[DAT_ThreatDialect] (
    [TicketNumber]                BIGINT       NOT NULL,
    [TicketType]                  VARCHAR (50) NOT NULL,
    [English]                     BIT          CONSTRAINT [DF_DAT_ThreatDialect_English] DEFAULT ((0)) NOT NULL,
    [RegionalAmerican]            BIT          CONSTRAINT [DF_DAT_ThreatDialect_RegionalAmerican] DEFAULT ((0)) NOT NULL,
    [RegionalAmericanDescription] VARCHAR (50) NULL,
    [ForeignAccent]               BIT          CONSTRAINT [DF_DAT_ThreatDialect_ForeignAccent] DEFAULT ((0)) NOT NULL,
    [ForeignAccentDescription]    VARCHAR (50) NULL,
    CONSTRAINT [PK_DAT_ThreatDialect] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ThreatDialect_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

