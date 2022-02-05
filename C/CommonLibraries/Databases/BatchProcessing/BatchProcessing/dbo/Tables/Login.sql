CREATE TABLE [dbo].[Login] (
    [LoginId]          INT             IDENTITY (1, 1) NOT NULL,
    [UserName]         VARCHAR (128)   NOT NULL,
    [EncrypedPassword] VARBINARY (128) NOT NULL,
    [LoginTypeId]      INT             DEFAULT ((1)) NOT NULL,
    [Notes]            VARCHAR (100)   NOT NULL,
    [Active] BIT NOT NULL DEFAULT (1)
    CONSTRAINT [PK_Login_LoginId] PRIMARY KEY CLUSTERED ([LoginId] ASC),
    [LastUpdated] DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_Login_LoginType] FOREIGN KEY ([LoginTypeId]) REFERENCES [dbo].[LoginType] ([LoginTypeId]),
);



GO
