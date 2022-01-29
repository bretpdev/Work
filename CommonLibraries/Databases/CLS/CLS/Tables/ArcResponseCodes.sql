CREATE TABLE [dbo].[ArcResponseCodes]
(
	[ArcResponseCodeId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ResponseCode] VARCHAR(5) NOT NULL, 
    CONSTRAINT [AK_ArcResponseCodes_ResponseCode] UNIQUE (ResponseCode), 
)
