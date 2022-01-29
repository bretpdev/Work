CREATE TABLE [dbo].[DAT_OddComputerBehavior] (
    [TicketNumber]                              BIGINT       NOT NULL,
    [TicketType]                                VARCHAR (50) NOT NULL,
    [EmailPhishingOrHoax]                       BIT          CONSTRAINT [DF_DAT_OddComputerBehavior_EmailPhishingOrHoax] DEFAULT ((0)) NOT NULL,
    [DenialOfService]                           BIT          CONSTRAINT [DF_DAT_OddComputerBehavior_DenialOfService] DEFAULT ((0)) NOT NULL,
    [UnexplainedAttemptToWriteToSystemFiles]    BIT          CONSTRAINT [DF_DAT_OddComputerBehavior_UnexplainedAttemptToWriteToSystemFiles] DEFAULT ((0)) NOT NULL,
    [UnexplainedModificationOrDeletionOfDate]   BIT          CONSTRAINT [DF_DAT_OddComputerBehavior_UnexplainedModificationOrDeletionOfDate] DEFAULT ((0)) NOT NULL,
    [UnexplainedModificationToFileLengthOrDate] BIT          CONSTRAINT [DF_DAT_OddComputerBehavior_UnexplainedModificationToFileLengthOrDate] DEFAULT ((0)) NOT NULL,
    [UnexplainedNewFilesOrUnfamiliarFileNames]  BIT          CONSTRAINT [DF_DAT_OddComputerBehavior_UnexplainedNewFilesOrUnfamiliarFileNames] DEFAULT ((0)) NOT NULL,
    [Malware]                                   BIT          CONSTRAINT [DF_DAT_OddComputerBehavior_Malware] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DAT_OddComputerBehavior] PRIMARY KEY CLUSTERED ([TicketNumber] ASC, [TicketType] ASC),
    CONSTRAINT [FK_DAT_OddComputerBehavior_DAT_Ticket] FOREIGN KEY ([TicketNumber], [TicketType]) REFERENCES [dbo].[DAT_Ticket] ([TicketNumber], [TicketType]) ON UPDATE CASCADE
);

