CREATE TABLE [webapi].[AccessLog]
(
	[AccessLogId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RelativeUrl] VARCHAR(2048), 
	[ApiTokenId] INT NULL,
	[UserTokenId] INT NULL,
	[RequestTimestamp] DATETIME NOT NULL DEFAULT GETDATE(),
	[SuccessfulAccess] BIT NOT NULL,
    CONSTRAINT [FK_AccessLog_ApiTokens] FOREIGN KEY ([ApiTokenId]) REFERENCES webapi.ApiTokens([ApiTokenId]),
	CONSTRAINT [FK_AccessLog_UserTokens] FOREIGN KEY ([UserTokenId]) REFERENCES webapi.UserTokens([UserTokenId])
)
