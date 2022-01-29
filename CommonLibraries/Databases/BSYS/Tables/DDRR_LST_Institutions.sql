CREATE TABLE [dbo].[DDRR_LST_Institutions] (
    [ID]   VARCHAR (50)  NOT NULL,
    [Name] VARCHAR (100) NOT NULL,
    [Type] CHAR (1)      NOT NULL,
    CONSTRAINT [PK_DDRR_LST_InstitutionNames] PRIMARY KEY CLUSTERED ([ID] ASC)
);

