CREATE TABLE [dbo].[DATHistory] (
    [TicketNumber]            BIGINT        NOT NULL,
    [TicketType]              VARCHAR (50)  NOT NULL,
    [UpdateDateTime]          DATETIME      NOT NULL,
    [SqlUserId]               INT           NOT NULL,
    [Status]                  VARCHAR (50)  NOT NULL,
    [StatusChangeDescription] VARCHAR (200) NULL,
    [UpdateText]              VARCHAR (MAX) NULL,
    CONSTRAINT [PK_DATHistory] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC, [UpdateDateTime] ASC),
    CONSTRAINT [FK_DATHistory_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

