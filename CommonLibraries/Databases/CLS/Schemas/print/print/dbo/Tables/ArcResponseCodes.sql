﻿CREATE TABLE [dbo].[ArcResponseCodes] (
    [ArcResponseCodeId] INT         IDENTITY (1, 1) NOT NULL,
    [ResponseCode]      VARCHAR (5) NOT NULL,
    PRIMARY KEY CLUSTERED ([ArcResponseCodeId] ASC),
    CONSTRAINT [AK_ArcResponseCodes_ResponseCode] UNIQUE NONCLUSTERED ([ResponseCode] ASC)
);

