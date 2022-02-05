CREATE TABLE [dbo].[QSTA_DAT_UserData] (
	UserDataId int not null IDENTITY,
    [RunDateTime]   DATETIME      NOT NULL,
    [Queue]         VARCHAR(8)  NOT NULL,
    [UserId]        VARCHAR(7)  NOT NULL,
    [Assigned]    INT NOT NULL,
    [Complete] INT        NOT NULL
    CONSTRAINT [PK_QSTA_DAT_UserData] PRIMARY KEY ([UserDataId]), 
    [Canceled] INT NOT NULL
);

