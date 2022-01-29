CREATE TABLE [dbo].[UserDat] (
    [UserID]    VARCHAR (50)  NOT NULL,
    [Password]  VARCHAR (100) CONSTRAINT [DF_UserDat_Password] DEFAULT ('') NOT NULL,
    [AuthLevel] SMALLINT      NOT NULL,
    [Valid]     BIT           CONSTRAINT [DF_UserDat_Valid] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_UserDat] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_UserDat_AuthList] FOREIGN KEY ([AuthLevel]) REFERENCES [dbo].[AuthList] ([AuthLevel]) ON UPDATE CASCADE
);

