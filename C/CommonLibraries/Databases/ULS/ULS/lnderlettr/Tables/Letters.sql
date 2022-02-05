CREATE TABLE [lnderlettr].[Letters] (
    [LettersId]       INT           IDENTITY (1, 1) NOT NULL,
    [BF_SSN]          VARCHAR (9)   NOT NULL,
    [II_LDR_VLD_ADR]  CHAR (1)      NOT NULL,
    [InLenderList]    BIT           NULL,
    [WF_ORG_LDR]      CHAR (8)      NOT NULL,
    [LetterCreatedAt] DATETIME      NULL,
    [ArcAddedAt]      DATETIME      NULL,
    [QueueClosedAt]   DATETIME      NULL,
    [AddedAt]         DATETIME      CONSTRAINT [DF_Letters_AddedAt] DEFAULT (getdate()) NOT NULL,
    [AddedBy]         VARCHAR (100) CONSTRAINT [DF_Letters_AddedBy] DEFAULT (suser_sname()) NOT NULL,
    [DeletedAt]       DATETIME      NULL,
    [DeletedBy]       VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([LettersId] ASC) WITH (FILLFACTOR = 95)
);

