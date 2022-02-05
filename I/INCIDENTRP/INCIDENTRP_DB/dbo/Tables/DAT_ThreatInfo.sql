CREATE TABLE [dbo].[DAT_ThreatInfo] (
    [TicketNumber]      BIGINT        NOT NULL,
    [TicketType]        VARCHAR (50)  NOT NULL,
    [WordingOfThreat]   VARCHAR (MAX) NULL,
    [NatureOfCall]      VARCHAR (MAX) NULL,
    [AdditionalRemarks] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_DAT_ThreatInformation] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ThreatInformation_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

