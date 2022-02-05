CREATE TABLE [dbo].[Marital_Status] (
    [marital_status_id] INT          IDENTITY (1, 1) NOT NULL,
    [status]            VARCHAR (20) NOT NULL,
    PRIMARY KEY CLUSTERED ([marital_status_id] ASC)
);

