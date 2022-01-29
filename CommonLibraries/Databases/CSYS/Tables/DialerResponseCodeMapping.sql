CREATE TABLE [dbo].[DialerResponseCodeMapping]
(
	[DialerResponseCodeMappingId] int not null IDENTITY,
    [ResultCode]   VARCHAR (2) NOT NULL,
    [Arc]          VARCHAR (5) NOT NULL,
    [Comment]      VARCHAR(1233)  NOT NULL,
    [ResponseCode] VARCHAR(5)    NOT NULL, 
    CONSTRAINT [PK_SCFU_ResultCodes_Arc_Comment] PRIMARY KEY ([DialerResponseCodeMappingId]), 
    CONSTRAINT [AK_SCFU_ResultCodes_Arc_Comment_ResultCode] UNIQUE (ResultCode)
)
