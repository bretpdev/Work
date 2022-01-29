CREATE TABLE [dbo].[Application_Source] (
    [application_source_id] INT           IDENTITY (1, 1) NOT NULL,
    [application_source]    VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Application_Source] PRIMARY KEY CLUSTERED ([application_source_id] ASC)
);

