﻿CREATE TABLE [dbo].[QSTA_DAT_UTDDL2] (
    [RECNUM]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [RUNTIMEDATE]     DATETIME      NOT NULL,
    [TARGET]          NVARCHAR (9)  NULL,
    [REGION]          NVARCHAR (5)  NULL,
    [QUEUE]           NVARCHAR (10) NULL,
    [ACCTNUM]         NVARCHAR (20) NULL,
    [TASK]            NVARCHAR (5)  NULL,
    [DIALERODR]       NVARCHAR (5)  NULL,
    [TASKS_INDICATOR] NVARCHAR (1)  NULL,
    [TIME_ZONE]       NVARCHAR (4)  NULL,
    [BORROWER_SSN]    NVARCHAR (9)  NULL,
    [BORROWER_WAGES]  MONEY         NULL,
    [BORROWER_GOODEI] NVARCHAR (1)  NULL,
    [BORROWER_INSAMT] MONEY         NULL,
    [BORROWER_OUTBAL] MONEY         NULL,
    [BORROWER_LSTPAY] DATETIME      NULL,
    [BORROWER_DUDAT]  DATETIME      NULL,
    [COHORT]          NVARCHAR (20) NULL,
    [LAST_TARGETATMP] DATETIME      NULL,
    [LAST_TARGETCNT]  DATETIME      NULL,
    [LAST_TARGETWORK] DATETIME      NULL,
    [CODE_TARGETRELA] NVARCHAR (1)  NULL,
    [PHONE_TYPE]      NVARCHAR (1)  NULL,
    [PHONE1]          NVARCHAR (20) NULL,
    [PHONE_TYPEALT]   NVARCHAR (1)  NULL,
    [PHONE2]          NVARCHAR (20) NULL,
    [TARGET_TYPEWK]   NVARCHAR (20) NULL,
    [PHONE3]          NVARCHAR (20) NULL,
    [DAYS_DELIQSKIP]  INT           NULL,
    [REGARD_ID]       NVARCHAR (20) NULL,
    [DUR1]            INT           NULL,
    [DUR2]            INT           NULL,
    [DUR3]            INT           NULL,
    [DUR5]            INT           NULL,
    [AGENT]           NVARCHAR (10) NULL,
    [TME]             DATETIME      NULL,
    [CODE]            NVARCHAR (5)  NULL,
    [ENTRYDATE]       DATETIME      NULL,
    [RECALLTIME]      DATETIME      NULL,
    [DAYSCNT]         INT           NULL,
    [PHONESTAT]       NVARCHAR (5)  NULL,
    [ZONEPHONE1]      NVARCHAR (1)  NULL,
    [ZONEPHONE2]      NVARCHAR (1)  NULL,
    [ZONEPHONE3]      NVARCHAR (1)  NULL,
    [PHONECNT1]       NVARCHAR (5)  NULL,
    [PHONECNT2]       NVARCHAR (5)  NULL,
    [PHONECNT3]       NVARCHAR (5)  NULL,
    [LASTTIME1]       DATETIME      NULL,
    [LASTTIME2]       DATETIME      NULL,
    [LASTTIME3]       DATETIME      NULL,
    [LASTSTAT1]       NVARCHAR (5)  NULL,
    [LASTSTAT2]       NVARCHAR (5)  NULL,
    [LASTSTAT3]       NVARCHAR (5)  NULL,
    [CURPHONE]        NVARCHAR (5)  NULL,
    [RECALLPHONE]     NVARCHAR (20) NULL,
    [DUR4]            INT           NULL,
    [JOBNAME]         NVARCHAR (20) NULL,
    [RECALLNUMBER]    NVARCHAR (20) NULL,
    [COUNTER]         INT           NULL,
    [JOBID]           NVARCHAR (20) NULL,
    [SHADOWJOB]       NVARCHAR (20) NULL,
    [FRSTTIME1]       DATETIME      NULL,
    [FRSTSTAT1]       NVARCHAR (20) NULL,
    [SCNDTIME1]       DATETIME      NULL,
    [SCNDSTAT1]       NVARCHAR (20) NULL,
    [THRDTIME1]       DATETIME      NULL,
    [THRDSTAT1]       NVARCHAR (20) NULL,
    [FRTHTIME1]       DATETIME      NULL,
    [FRTHSTAT1]       NVARCHAR (20) NULL,
    [FIFTTIME1]       DATETIME      NULL,
    [FIFTSTAT1]       NVARCHAR (20) NULL,
    [FRSTTIME2]       DATETIME      NULL,
    [FRSTSTAT2]       NVARCHAR (20) NULL,
    [SCNDTIME2]       DATETIME      NULL,
    [SCNDSTAT2]       NVARCHAR (20) NULL,
    [THRDTIME2]       DATETIME      NULL,
    [THRDSTAT2]       NVARCHAR (20) NULL,
    [FRTHTIME2]       DATETIME      NULL,
    [FRTHSTAT2]       NVARCHAR (20) NULL,
    [FIFTTIME2]       DATETIME      NULL,
    [FIFTSTAT2]       NVARCHAR (20) NULL,
    [FRSTTIME3]       DATETIME      NULL,
    [FRSTSTAT3]       NVARCHAR (20) NULL,
    [SCNDTIME3]       DATETIME      NULL,
    [SCNDSTAT3]       NVARCHAR (20) NULL,
    [THRDTIME3]       DATETIME      NULL,
    [THRDSTAT3]       NVARCHAR (20) NULL,
    [FRTHTIME3]       DATETIME      NULL,
    [FRTHSTAT3]       NVARCHAR (20) NULL,
    [FIFTTIME3]       DATETIME      NULL,
    [FIFTSTAT3]       NVARCHAR (20) NULL,
    CONSTRAINT [PK_QS_UTDDL2] PRIMARY KEY CLUSTERED ([RECNUM] ASC)
);

