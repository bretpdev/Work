﻿CREATE TABLE [dbo].[LockboxPaymentPosting] (
    [LockboxPaymentPosting] INT            IDENTITY (1, 1) NOT NULL,
    [BF_SSN]                CHAR (9)       NOT NULL,
    [LA_BR_RMT]             DECIMAL (8, 2) NOT NULL,
    [LF_BR_RMT_SCH_NUM]     CHAR (10)      NOT NULL,
    [LD_RMT_SCH_NUM_DPS]    DATETIME       NULL,
    [LC_RMT_STA]            CHAR (1)       NOT NULL,
    [LD_RMT_PST]            DATETIME       NULL,
    [LD_RMT_SPS]            DATETIME       NULL,
    [PC_TRY_SCH_SRC]        CHAR (3)       NULL,
    [DAYS_DIFFERENCE]       INT            NULL,
    PRIMARY KEY CLUSTERED ([LockboxPaymentPosting] ASC)
);

