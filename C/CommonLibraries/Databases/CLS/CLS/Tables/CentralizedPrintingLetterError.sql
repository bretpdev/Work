CREATE TABLE [dbo].[CentralizedPrintingLetterError] (
    [SeqNum]       INT      IDENTITY (1, 1) NOT NULL,
    [LetterSeqNum] INT      NOT NULL,
    [Detected]     DATETIME NOT NULL,
    [Handled]      DATETIME NULL,
    [RePrinted]    DATETIME NULL,
    CONSTRAINT [PK_CentralizedPrintingLetterError] PRIMARY KEY CLUSTERED ([SeqNum] ASC),
    CONSTRAINT [FK_CentralizedPrintingLetterError_CentralizedPrintingLetter] FOREIGN KEY ([LetterSeqNum]) REFERENCES [dbo].[CentralizedPrintingLetter] ([SeqNum]) ON DELETE CASCADE ON UPDATE CASCADE
);

