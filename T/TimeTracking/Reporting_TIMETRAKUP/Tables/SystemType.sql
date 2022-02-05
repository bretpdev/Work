CREATE TABLE [dbo].[SystemType] (
    [SystemTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [SystemType]   VARCHAR (25) NOT NULL,
    CONSTRAINT [PK_SystemType] PRIMARY KEY CLUSTERED ([SystemTypeId] ASC) WITH (FILLFACTOR = 95)
);

