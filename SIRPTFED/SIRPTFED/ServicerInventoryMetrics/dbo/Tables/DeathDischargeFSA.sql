﻿CREATE TABLE [dbo].[DeathDischargeFSA] (
    [DeathDischargeFSAId] INT         IDENTITY (1, 1) NOT NULL,
    [BF_SSN]              CHAR (9)    NOT NULL,
    [PF_REQ_ACT]          VARCHAR (5) NOT NULL,
    [LD_ATY_REQ_RCV]      DATETIME    NOT NULL,
    PRIMARY KEY CLUSTERED ([DeathDischargeFSAId] ASC)
);

