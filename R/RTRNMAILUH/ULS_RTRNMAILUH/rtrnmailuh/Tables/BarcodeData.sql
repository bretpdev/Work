CREATE TABLE [rtrnmailuh].[BarcodeData] (
    [BarcodeDataId]      INT          IDENTITY (1, 1) NOT NULL,
    [AccountIdentifier]  VARCHAR (10) NOT NULL,
    [LetterId]           VARCHAR (10) NOT NULL,
    [CreateDate]         DATETIME     NOT NULL,
    [ReceivedDate]       DATETIME     NOT NULL,
    [Address1]           VARCHAR (50) NULL,
    [Address2]           VARCHAR (50) NULL,
    [City]               VARCHAR (50) NULL,
    [State]              VARCHAR (50) NULL,
    [Zip]                VARCHAR (50) NULL,
    [Comment]            VARCHAR (360) NULL,
    [ProcessedAt]        DATETIME     NULL,
    [ArcAddProcessingId] INT          NULL,
    [AddedAt]            DATETIME     DEFAULT (getdate()) NOT NULL,
    [AddedBy]            VARCHAR (50) DEFAULT (suser_sname()) NOT NULL,
    [DeletedAt]          DATETIME     NULL,
    [DeletedBy]          VARCHAR (50) NULL,
    CONSTRAINT [PK_BarcodeData] PRIMARY KEY CLUSTERED ([BarcodeDataId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [AK_LetterLoaded] UNIQUE NONCLUSTERED ([AccountIdentifier] ASC, [LetterId] ASC, [CreateDate] ASC, [DeletedAt] ASC) WITH (FILLFACTOR = 95)
);

