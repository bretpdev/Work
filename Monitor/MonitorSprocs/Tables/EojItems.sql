CREATE TABLE [monitor].[EojItems]
(
	[EojItemId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[EojReportId] INT NOT NULL,
    [RunHistoryId] INT NOT NULL, 
    [Ssn] CHAR(9) NOT NULL, 
    [TaskControl] VARCHAR(30) NOT NULL, 
    [ActionRequest] VARCHAR(10) NOT NULL, 
    [R0CreateDate] DATETIME NULL, 
    [MonitorReason] VARCHAR(50) NOT NULL, 
    [OldMonthlyPayment] MONEY NULL, 
    [NewMonthlyPayment] MONEY NULL, 
    [ForcedDisclosure] BIT NULL, 
    [MaxIncrease] MONEY NULL, 
    [10CreateDate] DATETIME NULL, 
    [CancelReason] VARCHAR(1000) NULL, 
    CONSTRAINT [FK_EndOfJob_RunHistory] FOREIGN KEY ([RunHistoryId]) REFERENCES monitor.RunHistory([RunHistoryId]),
	CONSTRAINT [FK_EndOfJob_EojReports] FOREIGN KEY ([EojReportId]) REFERENCES monitor.EojReports([EojReportId]),
)
