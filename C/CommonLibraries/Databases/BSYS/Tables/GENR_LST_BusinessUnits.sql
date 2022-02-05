CREATE TABLE [dbo].[GENR_LST_BusinessUnits] (
    [BusinessUnit] NVARCHAR (50) NOT NULL,
    [PseudoBU]     CHAR (1)      NOT NULL,
    [LetterNo]     CHAR (1)      NULL,
    [Type]         NVARCHAR (50) NULL,
    [Parent]       NVARCHAR (50) NULL,
    [Abbreviation] NVARCHAR (50) NULL,
    CONSTRAINT [PK_GENR_LST_BusinessUnits_1] PRIMARY KEY CLUSTERED ([BusinessUnit] ASC)
);

