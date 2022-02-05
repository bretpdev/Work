CREATE TABLE [dbo].[CentralizedPrintingCompletion] (
    [PrintingCompletedFor] DATETIME NOT NULL,
    CONSTRAINT [PK_CentralizedPrintingCompletion] PRIMARY KEY CLUSTERED ([PrintingCompletedFor] ASC)
);

