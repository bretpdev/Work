CREATE TABLE [dbo].[CentralizedPrintingFaxError] (
    [SeqNum]    INT      IDENTITY (1, 1) NOT NULL,
    [FaxSeqNum] INT      NOT NULL,
    [Detected]  DATETIME NOT NULL,
    [Handled]   DATETIME NULL,
    CONSTRAINT [PK_CentralizedPrintingFaxError] PRIMARY KEY CLUSTERED ([SeqNum] ASC),
    CONSTRAINT [FK_CentralizedPrintingFaxError_CentralizedPrintingFax] FOREIGN KEY ([FaxSeqNum]) REFERENCES [dbo].[CentralizedPrintingFax] ([SeqNum]) ON DELETE CASCADE ON UPDATE CASCADE
);

