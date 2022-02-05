CREATE TABLE [dbo].[COST_DAT_TimeTracking_Trial_Run] (
    [TimeTrackingId]       INT             IDENTITY (1, 1) NOT NULL,
    [TaskDate]             DATE            NOT NULL,
    [Hours]                DECIMAL (18, 3) NOT NULL,
    [Sr]                   INT             NULL,
    [Sasr]                 INT             NULL,
    [Lts]                  INT             NULL,
    [Pmd]                  INT             NULL,
    [Project]              INT             NULL,
    [GenericMeetings]      VARCHAR (MAX)   CONSTRAINT [DF_COST_DAT_TimeTracking_DEV_GenericMeetings] DEFAULT ('') NULL,
    [BatchScripts]         VARCHAR (50)    CONSTRAINT [DF_COST_DAT_TimeTracking_DEV_BatchScripts] DEFAULT ('') NULL,
    [FsaCr]                VARCHAR (50)    CONSTRAINT [DF_COST_DAT_TimeTracking_DEV_FsaCr] DEFAULT ('') NULL,
    [BillingScript]        CHAR (1)        CONSTRAINT [DF_COST_DAT_TimeTracking_DEV_BillingScript] DEFAULT ('') NULL,
    [ConversionActivities] CHAR (1)        CONSTRAINT [DF_COST_DAT_TimeTracking_DEV_ConversionActivities] DEFAULT ('') NULL,
    [CostCenter]           VARCHAR (50)    CONSTRAINT [DF_COST_DAT_TimeTracking_DEV_CostCenter] DEFAULT ('') NULL,
    [Agent]                VARCHAR (50)    CONSTRAINT [DF_COST_DAT_TimeTracking_DEV_Agent] DEFAULT ('') NULL,
    [CostCenterId]         INT             NULL,
    [SqlUserId]            INT             NULL
);

