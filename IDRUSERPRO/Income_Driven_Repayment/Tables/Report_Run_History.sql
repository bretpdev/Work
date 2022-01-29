CREATE TABLE [dbo].[Report_Run_History] (
    [report_run_history_id]   INT           IDENTITY (1, 1) NOT NULL,
    [run_date]                DATETIME      NULL,
    [start_date]              DATETIME      NULL,
    [end_date]                DATETIME      NULL,
    [previous_run_history_id] INT           NULL,
    [run_by]                  NVARCHAR (50) CONSTRAINT [DF_Report_Run_History_run_by] DEFAULT (N'batchscripts') NULL,
    CONSTRAINT [PK_Report_Run_History] PRIMARY KEY CLUSTERED ([report_run_history_id] ASC),
    CONSTRAINT [FK_Report_Run_History_Report_Run_History] FOREIGN KEY ([report_run_history_id]) REFERENCES [dbo].[Report_Run_History] ([report_run_history_id])
);

