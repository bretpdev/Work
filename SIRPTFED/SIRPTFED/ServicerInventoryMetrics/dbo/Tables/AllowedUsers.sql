CREATE TABLE [dbo].[AllowedUsers] (
    [AllowedUserId] INT          IDENTITY (1, 1) NOT NULL,
    [AllowedUser]   VARCHAR (50) NOT NULL,
    [IsAdmin]       BIT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([AllowedUserId] ASC),
    UNIQUE NONCLUSTERED ([AllowedUser] ASC)
);

