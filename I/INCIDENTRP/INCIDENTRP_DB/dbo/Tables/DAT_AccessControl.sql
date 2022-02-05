CREATE TABLE [dbo].[DAT_AccessControl] (
    [TicketNumber]                             BIGINT       NOT NULL,
    [TicketType]                               VARCHAR (50) NOT NULL,
    [ImproperAccessWasGranted]                 BIT          CONSTRAINT [DF_DAT_AccessControl_ImproperAccessWasGranted] DEFAULT ((0)) NOT NULL,
    [SystemAccessWasNotTerminatedOrModified]   BIT          CONSTRAINT [DF_DAT_AccessControl_SystemAccessWasNotTerminatedOrModified] DEFAULT ((0)) NOT NULL,
    [PhysicalAccessWasNotTerminatedOrModified] BIT          CONSTRAINT [DF_DAT_AccessControl_PhysicalAccessWasNotTerminatedOrModified] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_AccessControl] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_AccessControl_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

