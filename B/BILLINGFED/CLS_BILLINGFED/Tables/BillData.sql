CREATE TABLE [billing].[BillData] (
    [BillDataIdId]    INT           IDENTITY (1, 1) NOT NULL,
    [SASFieldName]    VARCHAR (100) NOT NULL,
    [BillLabel]       VARCHAR (100) NULL,
    [XCoord]          FLOAT (53)    NOT NULL,
    [YCoord]          FLOAT (53)    NOT NULL,
    [VertialAlign]    INT           NULL,
    [HorizontalAlign] INT           NULL,
    [FontTypeId]      INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([BillDataIdId] ASC),
    CONSTRAINT [FK_BillData_ToFontType] FOREIGN KEY ([FontTypeId]) REFERENCES [billing].[FontType] ([FontTypeId])
);

