CREATE TABLE [dbo].[DAT_Fax] (
    [TicketNumber]                BIGINT       NOT NULL,
    [TicketType]                  VARCHAR (50) NOT NULL,
    [FaxNumber]                   VARCHAR (20) NULL,
    [Recipient]                   VARCHAR (50) NOT NULL,
    [IncorrectDocumentsWereFaxed] BIT          CONSTRAINT [DF_DAT_Fax_IncorrectDocumentsWereFaxed] DEFAULT ((0)) NOT NULL,
    [FaxNumberWasIncorrect]       BIT          CONSTRAINT [DF_DAT_Fax_FaxNumberWasIncorrect] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_Fax] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_Fax_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

