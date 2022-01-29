CREATE TABLE [dbo].[QueueBuilderFile] (
    [FileName]             VARCHAR (50) NOT NULL,
    [EmptyFileIsOk]        BIT          NOT NULL,
    [MissingFileIsOk]      BIT          NOT NULL,
    [ProcessMultipleFiles] BIT          NOT NULL,
    CONSTRAINT [PK_QueueBuilderFile] PRIMARY KEY CLUSTERED ([FileName] ASC)
);



