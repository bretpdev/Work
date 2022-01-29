﻿CREATE TABLE [dbo].[Tags] (
    [TagId] INT           IDENTITY (1, 1) NOT NULL,
    [Tag]   VARCHAR (100) NOT NULL UNIQUE,
    CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([TagId] ASC)
);
