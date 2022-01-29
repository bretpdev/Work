﻿CREATE TABLE [dbo].[QSTA_DAT_UTDDL4] (
    [RECNUM]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [RUNTIMEDATE]    DATETIME      NOT NULL,
    [BORROWER_SSN]   NVARCHAR (9)  NULL,
    [REGION]         NVARCHAR (5)  NULL,
    [QUEUE]          NVARCHAR (10) NULL,
    [ACCTNUM]        NVARCHAR (20) NULL,
    [DAYS_COMPLETE]  INT           NULL,
    [LAST_ATTEMPT]   DATETIME      NULL,
    [DAYS_DELIQUENT] INT           NULL,
    [LAST_CONTACT]   DATETIME      NULL,
    [RECP_RELATION]  NVARCHAR (20) NULL,
    [CODE_RELATION]  NVARCHAR (1)  NULL,
    [RECIP_ID]       NVARCHAR (9)  NULL,
    [TYPE_HMPH]      NVARCHAR (1)  NULL,
    [PHONE1]         NVARCHAR (20) NULL,
    [TYPE_ALTPH]     NVARCHAR (1)  NULL,
    [PHONE2]         NVARCHAR (20) NULL,
    [TYPE_WKPH]      NVARCHAR (1)  NULL,
    [PHONE3]         NVARCHAR (20) NULL,
    [NUMBER_LOANS]   INT           NULL,
    [AMOUNT_DUE]     MONEY         NULL,
    [BALANCE]        MONEY         NULL,
    [DATE_LASTPY]    DATETIME      NULL,
    [AUTH_3RD]       NVARCHAR (1)  NULL,
    [RECNUMBER]      INT           NULL,
    [DUR1]           INT           NULL,
    [DUR2]           INT           NULL,
    [DUR3]           INT           NULL,
    [DUR5]           INT           NULL,
    [COUNTER]        INT           NULL,
    [AGENT]          NVARCHAR (10) NULL,
    [TME]            DATETIME      NULL,
    [CODE]           NVARCHAR (5)  NULL,
    [ENTRYDATE]      DATETIME      NULL,
    [RECALLTIME]     DATETIME      NULL,
    [RECALLPHONE]    NVARCHAR (20) NULL,
    [DAYSCNT]        INT           NULL,
    [CURPHONE]       NVARCHAR (5)  NULL,
    [JOBNAME]        NVARCHAR (20) NULL,
    [PHONESTAT]      NVARCHAR (5)  NULL,
    [ZONEPHONE1]     NVARCHAR (1)  NULL,
    [ZONEPHONE2]     NVARCHAR (1)  NULL,
    [ZONEPHONE3]     NVARCHAR (1)  NULL,
    [PHONECNT1]      NVARCHAR (5)  NULL,
    [PHONECNT2]      NVARCHAR (5)  NULL,
    [PHONECNT3]      NVARCHAR (5)  NULL,
    [DUR4]           INT           NULL,
    [LASTTIME1]      DATETIME      NULL,
    [LASTSTAT1]      NVARCHAR (5)  NULL,
    [RECALLNUMBER]   NVARCHAR (20) NULL,
    [JOBID]          NVARCHAR (20) NULL,
    [SHADOWJOB]      NVARCHAR (20) NULL,
    [FRSTTIME1]      DATETIME      NULL,
    [FRSTSTAT1]      NVARCHAR (20) NULL,
    [SCNDTIME1]      DATETIME      NULL,
    [SCNDSTAT1]      NVARCHAR (20) NULL,
    [THRDTIME1]      DATETIME      NULL,
    [THRDSTAT1]      NVARCHAR (20) NULL,
    [FRTHTIME1]      DATETIME      NULL,
    [FRTHSTAT1]      NVARCHAR (20) NULL,
    [FIFTTIME1]      DATETIME      NULL,
    [FIFTSTAT1]      NVARCHAR (20) NULL,
    [FRSTTIME2]      DATETIME      NULL,
    [FRSTSTAT2]      NVARCHAR (20) NULL,
    [SCNDTIME2]      DATETIME      NULL,
    [SCNDSTAT2]      NVARCHAR (20) NULL,
    [THRDTIME2]      DATETIME      NULL,
    [THRDSTAT2]      NVARCHAR (20) NULL,
    [FRTHTIME2]      DATETIME      NULL,
    [FRTHSTAT2]      NVARCHAR (20) NULL,
    [FIFTTIME2]      DATETIME      NULL,
    [FIFTSTAT2]      NVARCHAR (20) NULL,
    [LASTTIME2]      DATETIME      NULL,
    [LASTSTAT2]      NVARCHAR (20) NULL,
    [FRSTTIME3]      DATETIME      NULL,
    [FRSTSTAT3]      NVARCHAR (20) NULL,
    [SCNDTIME3]      DATETIME      NULL,
    [SCNDSTAT3]      NVARCHAR (20) NULL,
    [THRDTIME3]      DATETIME      NULL,
    [THRDSTAT3]      NVARCHAR (20) NULL,
    [FRTHTIME3]      DATETIME      NULL,
    [FRTHSTAT3]      NVARCHAR (20) NULL,
    [FIFTTIME3]      DATETIME      NULL,
    [FIFTSTAT3]      NVARCHAR (20) NULL,
    [LASTTIME3]      DATETIME      NULL,
    [LASTSTAT3]      NVARCHAR (20) NULL,
    CONSTRAINT [PK_QS_UTDDL4] PRIMARY KEY CLUSTERED ([RECNUM] ASC)
);
