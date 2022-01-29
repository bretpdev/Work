CREATE TABLE [dbo].[QCTR_DAT_Issue] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [Subject]          VARCHAR (100) NULL,
    [Requester]        VARCHAR (20)  NULL,
    [Requested]        DATETIME      NULL,
    [DateofActivity]   DATETIME      NULL,
    [Required]         DATETIME      NULL,
    [Priority]         FLOAT (53)    NULL,
    [Issue]            TEXT          NULL,
    [History]          TEXT          NULL,
    [Resolution]       TEXT          NULL,
    [Type]             VARCHAR (50)  NULL,
    [PriorityCategory] VARCHAR (200) NULL,
    [PriorityUrgency]  VARCHAR (200) NULL,
    CONSTRAINT [PK_QCTR_DAT_Issue] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

