CREATE TABLE [dbo].[PRNT_DAT_Print] (
    [SeqNum]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [LetterID]             VARCHAR (10)  NOT NULL,
    [AccountNumber]        VARCHAR (20)  NOT NULL,
    [BusinessUnit]         NVARCHAR (50) NOT NULL,
    [Domestic]             CHAR (1)      NULL,
    [RequestedDate]        DATETIME      CONSTRAINT [DF_PRNT_DAT_Print_RequestedDate] DEFAULT (getdate()) NOT NULL,
    [StateMail2DDocSeqNum] BIGINT        NULL,
    [PrintDate]            DATETIME      NULL,
    [ActualPrintedTime]    DATETIME      NULL,
    CONSTRAINT [PK_PRNT_DAT_Print] PRIMARY KEY CLUSTERED ([SeqNum] ASC),
    CONSTRAINT [FK_PRNT_DAT_Print_GENR_LST_BusinessUnits] FOREIGN KEY ([BusinessUnit]) REFERENCES [dbo].[GENR_LST_BusinessUnits] ([BusinessUnit]) ON UPDATE CASCADE
);

