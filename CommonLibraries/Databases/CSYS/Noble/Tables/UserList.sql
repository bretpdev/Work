CREATE TABLE [Noble].[UserList] (
    [UserListId]   INT          IDENTITY (1, 1) NOT NULL,
    [Username]     VARCHAR (50) NULL,
    [TSR]          VARCHAR (4)  NULL,
    [Last_Updated] DATETIME     NULL,
    CONSTRAINT [PK__UserList__374B94111AA9E072] PRIMARY KEY CLUSTERED ([UserListId] ASC)
);

