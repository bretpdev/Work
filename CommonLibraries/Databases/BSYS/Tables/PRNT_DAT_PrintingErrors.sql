CREATE TABLE [dbo].[PRNT_DAT_PrintingErrors] (
    [SeqNum]           BIGINT   IDENTITY (1, 1) NOT NULL,
    [PrintSeqNum]      BIGINT   NOT NULL,
    [ErrorEncountered] DATETIME CONSTRAINT [DF_PRNT_DAT_PrintingErrors_ErrorEncountered] DEFAULT (getdate()) NOT NULL,
    [ErrorHandled]     DATETIME NULL,
    [ErrorPrinted]     DATETIME NULL,
    CONSTRAINT [PK_PRNT_DAT_PrintingErrors] PRIMARY KEY CLUSTERED ([SeqNum] ASC),
    CONSTRAINT [FK_PRNT_DAT_PrintingErrors_PRNT_DAT_Print] FOREIGN KEY ([PrintSeqNum]) REFERENCES [dbo].[PRNT_DAT_Print] ([SeqNum]) ON DELETE CASCADE ON UPDATE CASCADE
);

