CREATE TABLE [dbo].[DAT_Reporter] (
    [TicketNumber]   BIGINT        NOT NULL,
    [TicketType]     VARCHAR (50)  NOT NULL,
    [SqlUserId]      INT           NOT NULL,
    [BusinessUnitId] INT           NULL,
    [PhoneNumber]    VARCHAR (20)  NULL,
    [Location]       VARCHAR (100) NULL,
    CONSTRAINT [PK_DAT_Reporter] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_Reporter_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

