CREATE TABLE [dbo].[Values] (
    [ValueId] INT         IDENTITY (1, 1) NOT NULL,
    [StringValue] VARCHAR(8000) NOT NULL, 
    CONSTRAINT [PK_Values] PRIMARY KEY CLUSTERED ([ValueId] ASC), 
    CONSTRAINT [AK_Values_BinaryValue] UNIQUE (StringValue)
);

GO