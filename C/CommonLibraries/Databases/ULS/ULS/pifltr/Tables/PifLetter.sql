CREATE TABLE [pifltr].[PifLetter] (
    [PifLetterId]   INT          IDENTITY (1, 1) NOT NULL,
    [AccountNumber] CHAR (10)    NOT NULL,
    [UniqueId]      VARCHAR (20) NULL,
    [ArcAddedAt]    DATETIME     NULL,
    [PrintedAt]     DATETIME     NULL,
    [ImagedAt]      DATETIME     NULL,
    [AddedAt]       DATETIME     CONSTRAINT [DF_PifLetter_AddedAt] DEFAULT (getdate()) NOT NULL,
    [AddedBy]       VARCHAR (50) CONSTRAINT [DF_PifLetter_AddedBy] DEFAULT (suser_sname()) NOT NULL,
    [DeletedAt]     DATETIME     NULL,
    [DeletedBy]     VARCHAR (50) NULL,
    CONSTRAINT [PK_PifLetter] PRIMARY KEY CLUSTERED ([PifLetterId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [UcAcctUniqueId] UNIQUE NONCLUSTERED ([AccountNumber] ASC, [UniqueId] ASC) WITH (FILLFACTOR = 95)
);

