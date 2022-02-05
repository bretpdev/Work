CREATE TABLE [dbo].[DAT_ThreatBombInfo] (
    [TicketNumber]    BIGINT        NOT NULL,
    [TicketType]      VARCHAR (50)  NOT NULL,
    [Location]        VARCHAR (100) NULL,
    [DetonationTime]  VARCHAR (100) NULL,
    [Appearance]      VARCHAR (200) NULL,
    [WhoPlacedAndWhy] VARCHAR (200) NULL,
    [CallerName]      VARCHAR (50)  NULL,
    CONSTRAINT [PK_DAT_ThreatBombInfo] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_ThreatBombInfo_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

