CREATE TABLE [dbo].[TagAttributeValueMapping] (
    [TagAttributeValueMappingId] INT IDENTITY (1, 1) NOT NULL,
    [TagId]                      INT NOT NULL,
    [TagAttributeValueId]        INT NOT NULL,
    CONSTRAINT [PK_TagAttributeValueMapping] PRIMARY KEY CLUSTERED ([TagAttributeValueMappingId] ASC)
);

