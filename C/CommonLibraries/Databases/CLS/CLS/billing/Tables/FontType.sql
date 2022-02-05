CREATE TABLE [billing].[FontType] (
    [FontTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [FontType]   VARCHAR (50) NOT NULL,
    [EnumValue]  INT          NULL,
    [FontSize]   FLOAT (53)   NULL,
    [IsBold]     BIT          NULL,
    PRIMARY KEY CLUSTERED ([FontTypeId] ASC)
);

