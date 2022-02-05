CREATE TABLE [dbo].[APPLICATIONS] (
    [application_id]   INT           IDENTITY (1, 1) NOT NULL,
    [application_name] VARCHAR (100) NOT NULL,
    [access_key]       VARCHAR (50)  NULL,
    [starting_class]   VARCHAR (50)  NULL,
    [starting_file_id] INT           NULL,
    CONSTRAINT [PK_APPLICATIONS] PRIMARY KEY CLUSTERED ([application_id] ASC),
    CONSTRAINT [FK_APPLICATIONS_ASSOCIATED_FILES] FOREIGN KEY ([starting_file_id]) REFERENCES [dbo].[ASSOCIATED_FILES] ([associated_file_id])
);

