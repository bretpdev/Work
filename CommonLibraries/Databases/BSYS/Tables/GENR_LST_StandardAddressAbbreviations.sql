CREATE TABLE [dbo].[GENR_LST_StandardAddressAbbreviations] (
    [Abbreviation] VARCHAR (4)  NOT NULL,
    [FullText]     VARCHAR (20) NULL,
    [TypeKey]      VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_GENR_LST_StandardAddressAbbreviations] PRIMARY KEY CLUSTERED ([Abbreviation] ASC)
);

