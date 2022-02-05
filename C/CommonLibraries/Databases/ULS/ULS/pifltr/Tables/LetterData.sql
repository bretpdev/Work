CREATE TABLE [pifltr].[LetterData] (
    [LetterDataId]   INT           IDENTITY (1, 1) NOT NULL,
    [LetterId]       INT           NOT NULL,
    [FirstName]      VARCHAR (100) NULL,
    [LastName]       VARCHAR (100) NULL,
    [Address1]       VARCHAR (100) NULL,
    [Address2]       VARCHAR (100) NOT NULL,
    [City]           VARCHAR (50)  NULL,
    [State]          VARCHAR (15)  NULL,
    [Zip]            CHAR (5)      NULL,
    [Country]        VARCHAR (100) NOT NULL,
    [EffectiveDate]  VARCHAR (10)  NULL,
    [ConsolPif]      BIT           NULL,
    [CostCenterCode] VARCHAR (10)  NULL,
    [GuarDate]       VARCHAR (10)  NOT NULL,
    [GuarAmt]        VARCHAR (20)  NOT NULL,
    [LoanPgm]        VARCHAR (100) NOT NULL,
    [LoanSeq]        INT           NOT NULL,
    CONSTRAINT [PK_PifLetterData] PRIMARY KEY CLUSTERED ([LetterDataId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_PifLetterData_PifLetter] FOREIGN KEY ([LetterId]) REFERENCES [pifltr].[Letters] ([LettersId])
);

