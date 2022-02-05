CREATE TABLE [dbo].[PRNT_DAT_Fax] (
    [SeqNum]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [FaxNumber]           VARCHAR (50)  NOT NULL,
    [AccountNumber]       VARCHAR (20)  NOT NULL,
    [BusinessUnit]        NVARCHAR (50) NOT NULL,
    [LetterID]            VARCHAR (10)  NOT NULL,
    [RequestedDate]       DATETIME      CONSTRAINT [DF_PRNT_DAT_Fax_RequestedDate] DEFAULT (getdate()) NOT NULL,
    [CommentsAddedTo]     VARCHAR (10)  NOT NULL,
    [RightFaxHandle]      VARCHAR (50)  NULL,
    [FaxDate]             DATETIME      NULL,
    [FaxConfirmationDate] DATETIME      NULL,
    [FinalStatus]         VARCHAR (20)  NULL,
    CONSTRAINT [PK_PRNT_DAT_Fax] PRIMARY KEY CLUSTERED ([SeqNum] ASC),
    CONSTRAINT [FK_PRNT_DAT_Fax_GENR_LST_BusinessUnits] FOREIGN KEY ([BusinessUnit]) REFERENCES [dbo].[GENR_LST_BusinessUnits] ([BusinessUnit]) ON DELETE CASCADE ON UPDATE CASCADE
);

