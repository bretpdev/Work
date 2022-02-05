CREATE TABLE [dbo].[UserRequestTypes] (
    [UserRequestTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [UserId]            VARCHAR (50) NULL,
    [RequestTypeId]     INT          NULL,
    [Developer]         BIT          NULL,
    PRIMARY KEY CLUSTERED ([UserRequestTypeId] ASC),
    FOREIGN KEY ([RequestTypeId]) REFERENCES [dbo].[RequestTypes] ([RequestTypeId])
);

