CREATE TABLE [dbo].[Income_Source] (
    [income_source_id]          INT           IDENTITY (1, 1) NOT NULL,
    [income_source_description] VARCHAR (150) NOT NULL,
    CONSTRAINT [PK_Income_Source] PRIMARY KEY CLUSTERED ([income_source_id] ASC)
);

