CREATE TABLE [peps].[CLOSURE] (
    [CLOSURE_ID]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [RecordType]              VARCHAR (2)   NULL,
    [OpeId]                   VARCHAR (8)   NULL,
    [ChangeIndicator]         VARCHAR (1)   NULL,
    [ClosureDtCurrent]        VARCHAR (8)   NULL,
    [ClosureDtPrevious]       VARCHAR (8)   NULL,
    [HistoryCd]               VARCHAR (1)   NULL,
    [UnauthorizedLocationInd] VARCHAR (1)   NULL,
    [TuitionRecoveryFund]     VARCHAR (1)   NULL,
    [PerkinsInd]              VARCHAR (1)   NULL,
    [KnownAmount]             VARCHAR (9)   NULL,
    [StateBondInd]            VARCHAR (1)   NULL,
    [SchoolBondAmount]        VARCHAR (9)   NULL,
    [RecordHolderDescription] VARCHAR (50)  NULL,
    [VerifiedBy]              VARCHAR (70)  NULL,
    [CreatedOnDt]             VARCHAR (8)   NULL,
    [ModifiedDt]              VARCHAR (8)   NULL,
    [Filler]                  VARCHAR (256) NULL,
    [ProcessedAt]             DATETIME      NULL,
    [DeletedAt]               DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([CLOSURE_ID] ASC) WITH (FILLFACTOR = 95)
);

