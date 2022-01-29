CREATE TABLE [dbo].[DAT_Lock] (
    [TicketNumber] BIGINT       NOT NULL,
    [TicketType]   VARCHAR (50) NOT NULL,
    [SqlUserId]    INT          NOT NULL,
    CONSTRAINT [PK_DAT_Lock] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_Lock_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

