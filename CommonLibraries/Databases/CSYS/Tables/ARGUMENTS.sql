CREATE TABLE [dbo].[ARGUMENTS] (
    [argument_id]          INT           IDENTITY (1, 1) NOT NULL,
    [argument]             VARCHAR (20)  NOT NULL,
    [argument_description] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ARGUMENTS] PRIMARY KEY CLUSTERED ([argument_id] ASC)
);

