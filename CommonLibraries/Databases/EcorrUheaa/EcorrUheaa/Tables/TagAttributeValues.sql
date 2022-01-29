﻿CREATE TABLE [dbo].[TagAttributeValues] (
    [TagAttributeValueId] INT           IDENTITY (1, 1) NOT NULL,
    [Attribute]           VARCHAR (250) NOT NULL,
    [Value]               VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_TagAttributeValues] PRIMARY KEY CLUSTERED ([TagAttributeValueId] ASC) WITH (FILLFACTOR = 95),
    UNIQUE NONCLUSTERED ([Attribute] ASC) WITH (FILLFACTOR = 95)
);

