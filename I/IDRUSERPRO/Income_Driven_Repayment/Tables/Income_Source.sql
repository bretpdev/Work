CREATE TABLE [dbo].[Income_Source] (
    [income_source_id] INT NOT NULL,
    [income_source_description] VARCHAR(150) NULL,
    [income_source_friendly_description] VARCHAR(150) NULL, 
    CONSTRAINT [PK_Income_Source] PRIMARY KEY CLUSTERED ([income_source_id] ASC)
);

