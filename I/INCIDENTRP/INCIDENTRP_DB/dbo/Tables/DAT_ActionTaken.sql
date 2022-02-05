CREATE TABLE [dbo].[DAT_ActionTaken] (
    [TicketNumber]    BIGINT        NOT NULL,
    [TicketType]      VARCHAR (50)  NOT NULL,
    [Action]          VARCHAR (100) NOT NULL,
    [ActionDateTime]  DATETIME      NOT NULL,
    [PersonContacted] VARCHAR (100) NULL,
    CONSTRAINT [PK_DAT_ActionTaken] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC, [Action] ASC),
    CONSTRAINT [FK_DAT_ActionTaken_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE,
    CONSTRAINT [FK_DAT_ActionTaken_LST_Action] FOREIGN KEY ([Action]) REFERENCES [dbo].[LST_Action] ([Action]) ON UPDATE CASCADE
);

