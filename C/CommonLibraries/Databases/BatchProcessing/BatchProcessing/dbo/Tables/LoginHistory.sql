CREATE TABLE [dbo].[LoginHistory] (
	LoginHistoryId int IDENTITY,
    [UserName]        VARCHAR (128)    NOT NULL,
    [EncryptedPassword] VARBINARY (128) NOT NULL,
	[LoginTypeId] int not null,
    [Notes]             VARCHAR (100)   NOT NULL,
    [LastUpdated]       DATETIME        NOT NULL DEFAULT GETDATE(),
    [Requestor]         VARCHAR (150)    NOT NULL, 
    [Action] VARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_LoginHistory] PRIMARY KEY ([LoginHistoryId])
);


GO