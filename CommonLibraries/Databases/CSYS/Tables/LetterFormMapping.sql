CREATE TABLE [dbo].[LetterFormMapping] (
    [LetterFormMappingId] INT           IDENTITY (1, 1) NOT NULL,
    [LetterId]            VARCHAR (10)  NOT NULL,
    [FormPath]            VARCHAR (260) NOT NULL,
    [Form]                VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([LetterFormMappingId] ASC)
);



