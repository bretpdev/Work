CREATE TABLE [tcpapns].[FileProcessing] (
    [FileProcessingId] INT           IDENTITY (1, 1) NOT NULL,
    [GroupKey]         VARCHAR (50)  NULL,
    [SourceFile]       VARCHAR (100) NOT NULL,
	[LineData]         VARCHAR (1200) NULL,
	[HasConsentArc]	   BIT           NOT NULL DEFAULT(0),
	[AccountNumber]    VARCHAR (10)  NULL,
	[Phone]            VARCHAR (20)  NULL,
	[RecordDate]       VARCHAR (10)  NULL,
	[MobileIndicator]  BIT           NULL,
	[ProcessedOn]      DATETIME      NULL,
	[ArcAddProcessingId] BIGINT      NULL,
    [AddedBy]          VARCHAR (50)  NOT NULL,
    [AddedAt]          DATETIME      NOT NULL,
    [DeletedAt]        DATETIME      NULL,
    [DeletedBy]        VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([FileProcessingId] ASC) WITH (FILLFACTOR = 95)
);
