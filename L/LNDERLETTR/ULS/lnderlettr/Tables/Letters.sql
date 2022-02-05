CREATE TABLE [lnderlettr].[Letters] (
    [LettersId]          INT           IDENTITY (1, 1) NOT NULL,
    [BF_SSN]             CHAR (9)      NOT NULL,
    [II_LDR_VLD_ADR]     CHAR (1)      NOT NULL,
    [LDR_STR_ADR_1]      VARCHAR (30)  NOT NULL,
    [InLenderList]       BIT           NULL,
    [WF_ORG_LDR]         CHAR (8)      NOT NULL,
    [LetterCreatedAt]    DATETIME      NULL,
    [ArcAddProcessingId] BIGINT        NULL,
    [QueueClosedAt]      DATETIME      NULL,
    [AddedAt]            DATETIME      CONSTRAINT [DF_Letters_AddedAt] DEFAULT (getdate()) NOT NULL,
    [AddedBy]            VARCHAR (100) CONSTRAINT [DF_Letters_AddedBy] DEFAULT (suser_sname()) NOT NULL,
    [DeletedAt]          DATETIME      NULL,
    [DeletedBy]          VARCHAR (100) NULL,
    [ErroredAt]          DATETIME      NULL,
    [Population]		 INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([LettersId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_Letters_ArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [dbo].[ArcAddProcessing] ([ArcAddProcessingId])
);

