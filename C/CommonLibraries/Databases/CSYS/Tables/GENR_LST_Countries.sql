CREATE TABLE [dbo].[GENR_LST_Countries] (
    [country_id] INT          IDENTITY (1, 1) NOT NULL,
    [code]       NCHAR (2)    NOT NULL,
    [name]       VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GENR_LST_Countries] PRIMARY KEY CLUSTERED ([country_id] ASC)
);

