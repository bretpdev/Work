CREATE TABLE [dbo].[RequestTypes] (
    [RequestTypeId] INT         IDENTITY (1, 1) NOT NULL,
    [RequestType]   VARCHAR (6) NOT NULL,
    PRIMARY KEY CLUSTERED ([RequestTypeId] ASC)
);

