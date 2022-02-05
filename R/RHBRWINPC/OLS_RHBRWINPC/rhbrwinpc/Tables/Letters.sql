CREATE TABLE [rhbrwinpc].[Letters] (
    [LettersId]          INT           IDENTITY (1, 1) NOT NULL,
    [AccountNumber]      VARCHAR (10)  NOT NULL,
    [FirstName]          VARCHAR (20)  NOT NULL,
    [LastName]           VARCHAR (30)  NOT NULL,
    [Address1]           VARCHAR (40)  NOT NULL,
    [Address2]           VARCHAR (40)  NOT NULL,
    [City]               VARCHAR (20)  NOT NULL,
    [State]              VARCHAR (2)   NOT NULL,
    [Zip]                VARCHAR (17)  NOT NULL,
    [Country]            VARCHAR (25)  NOT NULL,
    [PrintedAt]          DATETIME      NULL,
    [ArcAddProcessingId] BIGINT        NULL,
    [AddedAt]            DATETIME      CONSTRAINT [DF_Letters_AddedAt] DEFAULT (getdate()) NOT NULL,
    [AddedBy]            VARCHAR (100) CONSTRAINT [DF_Letters_AddedBy] DEFAULT (suser_sname()) NOT NULL,
    [DeletedAt]          DATETIME      NULL,
    [DeletedBy]          VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([LettersId] ASC) WITH (FILLFACTOR = 95)
);

