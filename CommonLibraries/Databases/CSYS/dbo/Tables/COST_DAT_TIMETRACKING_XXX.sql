CREATE TABLE [dbo].[COST_DAT_TIMETRACKING_XXX] (
    [TimeTrackingId]       INT             IDENTITY (1, 1) NOT NULL,
    [TaskDate]             DATE            NOT NULL,
    [Hours]                DECIMAL (18, 3) NOT NULL,
    [Sr]                   INT             NULL,
    [Sasr]                 INT             NULL,
    [Lts]                  INT             NULL,
    [Pmd]                  INT             NULL,
    [Project]              INT             NULL,
    [GenericMeetings]      VARCHAR (MAX)   NULL,
    [BatchScripts]         VARCHAR (50)    NULL,
    [FsaCr]                VARCHAR (50)    NULL,
    [BillingScript]        CHAR (1)        NULL,
    [ConversionActivities] CHAR (1)        NULL,
    [CostCenter]           VARCHAR (50)    NULL,
    [Agent]                VARCHAR (50)    NULL,
    [CostCenterId]         INT             NULL,
    [SqlUserId]            INT             NULL
);

