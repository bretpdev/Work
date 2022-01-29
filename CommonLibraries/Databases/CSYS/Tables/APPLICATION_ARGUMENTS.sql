CREATE TABLE [dbo].[APPLICATION_ARGUMENTS] (
    [application_argument_id] INT IDENTITY (1, 1) NOT NULL,
    [application_id]          INT NOT NULL,
    [argument_id]             INT NOT NULL,
    [argument_order]          INT NOT NULL,
    CONSTRAINT [PK_APPLICATION_ARGUMENTS] PRIMARY KEY CLUSTERED ([application_argument_id] ASC),
    CONSTRAINT [FK_APPLICATION_ARGUMENTS_APPLICATIONS] FOREIGN KEY ([application_id]) REFERENCES [dbo].[APPLICATIONS] ([application_id]),
    CONSTRAINT [FK_APPLICATION_ARGUMENTS_ARGUMENTS] FOREIGN KEY ([argument_id]) REFERENCES [dbo].[ARGUMENTS] ([argument_id])
);

