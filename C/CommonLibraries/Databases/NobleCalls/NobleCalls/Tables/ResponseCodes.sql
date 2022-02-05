CREATE TABLE [dbo].[ResponseCodes]
(
	[ResponseCodeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ResponseCode] VARCHAR(5) NULL, 
    CONSTRAINT [AK_ResponseCodes_ResponseCode] UNIQUE (ResponseCode)
)
