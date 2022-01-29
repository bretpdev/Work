CREATE TABLE [dbo].[COST_TMP_BatchScriptTimeTracking] (
    [BatchScriptTimeTrackingId] INT           IDENTITY (1, 1) NOT NULL,
    [Script]                    VARCHAR (100) NOT NULL,
    [MinutesToRun]              INT           NOT NULL,
    [TimesRunEachMonth]         INT           NOT NULL,
    [TotalMinutesEachMonth]     INT           NOT NULL,
    [PercentOfTotalMinutes]     FLOAT (53)    NOT NULL,
    CONSTRAINT [PK_COST_TMP_BatchScriptTimeTracking] PRIMARY KEY CLUSTERED ([BatchScriptTimeTrackingId] ASC)
);

