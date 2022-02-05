CREATE TABLE [mssasgndft].[RangeAssignment] (
    [RangeAssignmentId] INT          IDENTITY (1, 1) NOT NULL, 
    [AesId] VARCHAR(7) NOT NULL,
    [UserId]            INT          NOT NULL,
    [BeginRange]        INT          NOT NULL,
    [EndRange]          INT          NOT NULL,
    [AddedOn]           DATETIME     DEFAULT (getdate()) NOT NULL,
    [AddedBy]           VARCHAR (50) DEFAULT (suser_sname()) NOT NULL
);