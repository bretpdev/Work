CREATE TABLE [dbo].[PRNT_DAT_FaxingErrors] (
    [SeqNum]           BIGINT   IDENTITY (1, 1) NOT NULL,
    [FaxSeqNum]        BIGINT   NOT NULL,
    [ErrorEncountered] DATETIME CONSTRAINT [DF_PRNT_DAT_FaxingErrors_ErrorEncountered] DEFAULT (getdate()) NOT NULL,
    [ErrorHandled]     DATETIME NULL,
    CONSTRAINT [FK_PRNT_DAT_FaxingErrors_PRNT_DAT_Fax] FOREIGN KEY ([FaxSeqNum]) REFERENCES [dbo].[PRNT_DAT_Fax] ([SeqNum]) ON DELETE CASCADE ON UPDATE CASCADE
);

