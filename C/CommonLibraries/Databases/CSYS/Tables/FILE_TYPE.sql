CREATE TABLE [dbo].[FILE_TYPE] (
    [file_type_id] INT          IDENTITY (1, 1) NOT NULL,
    [file_type]    VARCHAR (15) NOT NULL,
    CONSTRAINT [PK_FILE_TYPE] PRIMARY KEY CLUSTERED ([file_type_id] ASC)
);

