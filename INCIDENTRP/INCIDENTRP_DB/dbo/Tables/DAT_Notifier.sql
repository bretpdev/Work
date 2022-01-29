CREATE TABLE [dbo].[DAT_Notifier] (
    [TicketNumber]      BIGINT        NOT NULL,
    [TicketType]        VARCHAR (50)  NOT NULL,
    [Type]              VARCHAR (15)  NULL,
    [OtherType]         VARCHAR (50)  NULL,
    [Method]            VARCHAR (10)  NULL,
    [OtherMethod]       VARCHAR (50)  NULL,
    [Name]              VARCHAR (100) NULL,
    [EmailAddress]      VARCHAR (200) NULL,
    [PhoneNumber]       VARCHAR (20)  NULL,
    [Relationship]      VARCHAR (20)  NULL,
    [OtherRelationship] VARCHAR (50)  NULL,
    CONSTRAINT [PK_DAT_Notifier] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_Notifier_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE,
    CONSTRAINT [FK_DAT_Notifier_LST_NotificationMethod] FOREIGN KEY ([Method]) REFERENCES [dbo].[LST_NotificationMethod] ([Method]) ON UPDATE CASCADE,
    CONSTRAINT [FK_DAT_Notifier_LST_NotifierType] FOREIGN KEY ([Type]) REFERENCES [dbo].[LST_NotifierType] ([Notifier]) ON UPDATE CASCADE,
    CONSTRAINT [FK_DAT_Notifier_LST_Relationship] FOREIGN KEY ([Relationship]) REFERENCES [dbo].[LST_Relationship] ([Relationship]) ON UPDATE CASCADE
);

