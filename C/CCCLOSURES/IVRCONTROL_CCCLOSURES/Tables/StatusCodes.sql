CREATE TABLE [dbo].[StatusCodes] (
    [StatusCodeId]          INT           IDENTITY (1, 1) NOT NULL,
    [StatusCode]            VARCHAR (5)   NOT NULL,
    [StatusCodeName]        VARCHAR (20)  NOT NULL,
    [StatusCodeDescription] VARCHAR (MAX) NULL,
    [AddedAt]               DATETIME      DEFAULT (getdate()) NOT NULL,
    [AddedBy]               VARCHAR (50)  NOT NULL,
    [UpdatedAt]             DATETIME      NULL,
    [UpdatedBy]             VARCHAR (50)  NULL,
    [DeletedAt]             DATETIME      NULL,
    [DeletedBy]             VARCHAR (50)  NULL,
    CONSTRAINT [PK_StatusCodes] PRIMARY KEY CLUSTERED ([StatusCodeId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [AK_StatusCodes_StatusCodeName] UNIQUE NONCLUSTERED ([StatusCodeName] ASC) WITH (FILLFACTOR = 95)
);

