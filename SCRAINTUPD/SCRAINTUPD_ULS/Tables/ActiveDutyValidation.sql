﻿CREATE TABLE [scra].[ActiveDutyValidation] (
    [KEYID]                   INT          IDENTITY (1, 1) NOT NULL,
    [TALLY]                   INT          NOT NULL,
    [DF_SPE_ACC_ID]           VARCHAR (10) NULL,
    [BF_SSN]                  CHAR (9)     NOT NULL,
    [DM_PRS_1]                VARCHAR (20) NULL,
    [DM_PRS_MID]              VARCHAR (20) NULL,
    [DM_PRS_LST]              VARCHAR (26) NULL,
    [DD_BRT]                  DATE         NULL,
    [ACTIVE_DUTY_STATUS_DATE] DATE         NULL,
    [FILENAME]                VARCHAR (14)  NOT NULL,
    [SSIS_DOB]                VARCHAR (8)  NULL,
    [SSIS_ADSD]               VARCHAR (8)  NULL,
    [SSIS_PLACEHOLDER]        VARCHAR (18) NULL,
    PRIMARY KEY CLUSTERED ([KEYID] ASC) WITH (FILLFACTOR = 95)
);

