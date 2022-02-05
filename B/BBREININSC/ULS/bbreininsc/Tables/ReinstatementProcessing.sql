CREATE TABLE [bbreininsc].[ReinstatementProcessing] (
    [RecordId]    INT          IDENTITY (1, 1) NOT NULL,
    [BF_SSN]      CHAR (9)     NOT NULL,
    [LN_SEQ]      INT          NOT NULL,
    [LD_BIL_DU]   DATE         NOT NULL,
    [RecordType]  VARCHAR (3)  NOT NULL,
    [ProcessedAt] DATETIME     NULL,
    [CreatedAt]   DATETIME     NOT NULL,
    [CreatedBy]   VARCHAR (50) NOT NULL,
    [DeletedAt]   DATETIME     NOT NULL,
    [DeletedBy]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ReinstatementProcessing] PRIMARY KEY CLUSTERED ([RecordId] ASC) WITH (FILLFACTOR = 95)
);

