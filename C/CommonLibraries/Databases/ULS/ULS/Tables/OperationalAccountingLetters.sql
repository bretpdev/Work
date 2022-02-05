CREATE TABLE [dbo].[OperationalAccountingLetters] (
    [LetterTypeId]     INT           IDENTITY (1, 1) NOT NULL,
    [LetterType]       VARCHAR (25)  NOT NULL,
    [Letter]           VARCHAR (150) NOT NULL,
    [HasPaymentSource] BIT           NULL,
    [IsCompany]        BIT           NULL,
    [AddedAt]          DATETIME      CONSTRAINT [DF_OperationalAccountingLetters_AddedAt] DEFAULT (getdate()) NOT NULL,
    [AddedBy]          VARCHAR (100) CONSTRAINT [DF_OperationalAccountingLetters_AddedBy] DEFAULT (suser_sname()) NOT NULL,
    [DeletedAt]        DATETIME      NULL,
    [DeletedBy]        VARCHAR (100) NULL,
    CONSTRAINT [PK_OperationalAccountingLetters] PRIMARY KEY CLUSTERED ([LetterTypeId] ASC)
);

