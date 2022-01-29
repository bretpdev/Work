CREATE TABLE [monitor].[EojItems] (
    [EojItemId]         INT            IDENTITY (1, 1) NOT NULL,
    [EojReportId]       INT            NOT NULL,
    [RunHistoryId]      INT            NOT NULL,
    [Ssn]               CHAR (9)       NOT NULL,
    [TaskControl]       VARCHAR (30)   NOT NULL,
    [ActionRequest]     VARCHAR (10)   NOT NULL,
    [R0CreateDate]      DATETIME       NULL,
    [MonitorReason]     VARCHAR (50)   NOT NULL,
    [OldMonthlyPayment] MONEY          NULL,
    [NewMonthlyPayment] MONEY          NULL,
    [ForcedDisclosure]  BIT            NULL,
    [MaxIncrease]       MONEY          NULL,
    [10CreateDate]      DATETIME       NULL,
    [CancelReason]      VARCHAR (1000) NULL,
    CONSTRAINT [PK__EojItems__8E10E81276FBBDBC] PRIMARY KEY CLUSTERED ([EojItemId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_EndOfJob_EojReports] FOREIGN KEY ([EojReportId]) REFERENCES [monitor].[EojReports] ([EojReportId]),
    CONSTRAINT [FK_EndOfJob_RunHistory] FOREIGN KEY ([RunHistoryId]) REFERENCES [monitor].[RunHistory] ([RunHistoryId])
);

