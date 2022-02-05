CREATE TABLE [dbo].[ResponseCodes] (
    [ResponseCodeId] INT         IDENTITY (1, 1) NOT NULL,
    [ResponseCode]   VARCHAR (5) NULL,
    PRIMARY KEY CLUSTERED ([ResponseCodeId] ASC),
    CONSTRAINT [AK_ResponseCodes_ResponseCode] UNIQUE NONCLUSTERED ([ResponseCode] ASC)
);

