CREATE TABLE [dbo].[GENR_LST_AddressSources] (
    [source_id]   INT          IDENTITY (1, 1) NOT NULL,
    [code]        NCHAR (2)    NOT NULL,
    [description] VARCHAR (35) NOT NULL,
    CONSTRAINT [PK_GENR_LST_AddressSources] PRIMARY KEY CLUSTERED ([source_id] ASC)
);

