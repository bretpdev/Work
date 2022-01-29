CREATE TABLE [dbo].[UpdateLog] (
    [UpdateLogId]       INT          IDENTITY (1, 1) NOT NULL,
    [TimeTrackingId]    INT          NOT NULL, 
    [Region] VARCHAR(11) NOT NULL, 
    [StartTimeOldValue] DATETIME     NOT NULL,
    [StartTimeNewValue] DATETIME     NOT NULL,
    [EndTimeOldValue]   DATETIME     NULL,
    [EndTimeNewValue]   DATETIME     NULL,
    [UpdateBy]          INT          NOT NULL,
    [UpdatedAt]         DATETIME     DEFAULT (getdate()) NOT NULL, 
    PRIMARY KEY CLUSTERED ([UpdateLogId] ASC) WITH (FILLFACTOR = 95)
);


