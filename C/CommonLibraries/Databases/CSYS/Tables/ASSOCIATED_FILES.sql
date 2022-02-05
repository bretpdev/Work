CREATE TABLE [dbo].[ASSOCIATED_FILES] (
    [associated_file_id]   INT           IDENTITY (1, 1) NOT NULL,
    [application_id]       INT           NOT NULL,
    [source_path]          VARCHAR (256) NOT NULL,
    [destination_path]     VARCHAR (256) NULL,
    [file_type_id]         INT           NOT NULL,
    [associated_file_name] VARCHAR (50)  NULL,
    CONSTRAINT [PK_ASSOCIATED_FILES] PRIMARY KEY CLUSTERED ([associated_file_id] ASC),
    CONSTRAINT [FK_ASSOCIATED_FILES_APPLICATIONS] FOREIGN KEY ([application_id]) REFERENCES [dbo].[APPLICATIONS] ([application_id]),
    CONSTRAINT [FK_ASSOCIATED_FILES_FILE_TYPE] FOREIGN KEY ([file_type_id]) REFERENCES [dbo].[FILE_TYPE] ([file_type_id])
);

