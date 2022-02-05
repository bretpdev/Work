CREATE TABLE [dbo].[DAT_DisposalDestruction] (
    [TicketNumber]                                            BIGINT       NOT NULL,
    [TicketType]                                              VARCHAR (50) NOT NULL,
    [ElectronicMediaRecordsWereDestroyedInError]              BIT          CONSTRAINT [DF_DAT_DisposalDestruction_ElectronicMediaRecordsWereDestroyedInError] DEFAULT ((0)) NOT NULL,
    [ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod] BIT          CONSTRAINT [DF_DAT_DisposalDestruction_ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod] DEFAULT ((0)) NOT NULL,
    [MicrofilmWithRecordsWasDestroyedInError]                 BIT          CONSTRAINT [DF_DAT_DisposalDestruction_MicrofilmWithRecordsWasDestroyedInError] DEFAULT ((0)) NOT NULL,
    [MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod]    BIT          CONSTRAINT [DF_DAT_DisposalDestruction_MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod] DEFAULT ((0)) NOT NULL,
    [PaperRecordsWereDestroyedInError]                        BIT          CONSTRAINT [DF_DAT_DisposalDestruction_PaperRecordsWereDestroyedInError] DEFAULT ((0)) NOT NULL,
    [PaperRecordsWereDestroyedUsingIncorrectMethod]           BIT          CONSTRAINT [DF_DAT_DisposalDestruction_PaperRecordsWereDestroyedUsingIncorrectMethod] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_DisposalDestruction] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_DisposalDestruction_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

