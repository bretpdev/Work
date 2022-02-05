CREATE TABLE [billing].[SpecialMessages] (
    [SpecialMessageId]          INT           IDENTITY (1, 1) NOT NULL,
    [ReportNumber]              INT           NOT NULL,
    [FirstSpecialMessageTitle]  VARCHAR (100) NULL,
    [FirstSpecialMessage]       VARCHAR (500) NULL,
    [SecondSpecialMessageTitle] VARCHAR (100) NULL,
    [SecondSpecialMessage]      VARCHAR (500) NULL,
    [Message1XCoord]            FLOAT (53)    NULL,
    [Message1YCoord]            FLOAT (53)    NULL,
    [Message1FontTypeId]        INT           NULL,
    [Message2XCoord]            FLOAT (53)    NULL,
    [Message2YCoord]            FLOAT (53)    NULL,
    [Message2FontTypeId]        INT           NULL,
    PRIMARY KEY CLUSTERED ([SpecialMessageId] ASC),
    CONSTRAINT [FK_SpecialMessages_ToFontType] FOREIGN KEY ([Message1FontTypeId]) REFERENCES [billing].[FontType] ([FontTypeId]),
    CONSTRAINT [FK_SpecialMessages_ToFontType1] FOREIGN KEY ([Message2FontTypeId]) REFERENCES [billing].[FontType] ([FontTypeId])
);

