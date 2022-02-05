CREATE TABLE [dbo].[TimeOff] (
    [TimeOffId] INT          IDENTITY (1, 1) NOT NULL,
    [Date]      DATETIME     NULL,
    [UserId]    VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([TimeOffId] ASC)
);

