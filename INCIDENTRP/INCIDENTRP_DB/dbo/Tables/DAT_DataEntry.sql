CREATE TABLE [dbo].[DAT_DataEntry] (
    [TicketNumber]                     BIGINT       NOT NULL,
    [TicketType]                       VARCHAR (50) NOT NULL,
    [IncorrectInformationWasAdded]     BIT          CONSTRAINT [DF_DAT_DataEntry_IncorrectInformationWasAdded] DEFAULT ((0)) NOT NULL,
    [InformationWasIncorrectlyChanged] BIT          CONSTRAINT [DF_DAT_DataEntry_InformationWasIncorrectlyChanged] DEFAULT ((0)) NOT NULL,
    [InformationWasIncorrectlyDeleted] BIT          CONSTRAINT [DF_DAT_DataEntry_InformationWasIncorrectlyDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_DataEntry] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_DataEntry_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType])
);

