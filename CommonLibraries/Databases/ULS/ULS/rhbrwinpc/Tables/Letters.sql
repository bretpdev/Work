CREATE TABLE [rhbrwinpc].[Letters] (
    [LettersId]     INT           IDENTITY (1, 1) NOT NULL,
    [DF_SPE_ACC_ID] VARCHAR (10)  NOT NULL,
    [DM_PRS_1]      VARCHAR (20)  NOT NULL,
    [DM_PRS_LST]    VARCHAR (30)  NOT NULL,
    [DX_STR_ADR_1]  VARCHAR (40)  NOT NULL,
    [DX_STR_ADR_2]  VARCHAR (40)  NOT NULL,
    [DM_CT]         VARCHAR (20)  NOT NULL,
    [DC_DOM_ST]     VARCHAR (2)   NOT NULL,
    [DF_ZIP]        VARCHAR (17)  NOT NULL,
    [DM_FGN_CNY]    VARCHAR (25)  NOT NULL,
    [PrintedAt]     DATETIME      NULL,
    [ArcAddedAt]    DATETIME      NULL,
    [AddedAt]       DATETIME      CONSTRAINT [DF_Letters_AddedAt] DEFAULT (getdate()) NOT NULL,
    [AddedBy]       VARCHAR (100) CONSTRAINT [DF_Letters_AddedBy] DEFAULT (suser_sname()) NOT NULL,
    [DeletedAt]     DATETIME      NULL,
    [DeletedBy]     VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([LettersId] ASC) WITH (FILLFACTOR = 95)
);

