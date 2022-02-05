CREATE TABLE [mssasgndft].[RangeHistory] (
    [RangeHistoryId]    INT          IDENTITY (1, 1) NOT NULL,
    [RangeAssignmentId] INT          NOT NULL,
    [AesId]             VARCHAR (7)  NOT NULL,
    [UserId]            INT          NOT NULL,
    [BeginRange]        INT          NOT NULL,
    [EndRange]          INT          NOT NULL,
    [AddedOn]           DATETIME     NOT NULL,
    [AddedBy]           VARCHAR (50) NOT NULL,
    [DeletedAt]         DATETIME     DEFAULT (getdate()) NOT NULL,
    [DeletedBy]         VARCHAR (50) DEFAULT (suser_sname()) NOT NULL,
    PRIMARY KEY CLUSTERED ([RangeHistoryId] ASC) WITH (FILLFACTOR = 95)
);

