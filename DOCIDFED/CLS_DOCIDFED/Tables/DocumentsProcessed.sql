CREATE TABLE [docid].[DocumentsProcessed] (
    [DocumentProcessedId] INT          IDENTITY (1, 1) NOT NULL,
    [AccountIdentifier]   VARCHAR (10) NULL,
    [DocumentsId]         INT          NULL,
    [SourceId]            INT          NULL,
    [ArcAddProcessingId]  BIGINT       NULL,
    [AddedAt]             DATETIME     DEFAULT (getdate()) NOT NULL,
    [AddedBy]             VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([DocumentProcessedId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_DocumentsProcessed_ArcAddProcessing] FOREIGN KEY ([ArcAddProcessingId]) REFERENCES [dbo].[ArcAddProcessing] ([ArcAddProcessingId]),
    CONSTRAINT [FK_DocumentsProcessed_Documents] FOREIGN KEY ([DocumentsId]) REFERENCES [docid].[Documents] ([DocumentsId]),
    CONSTRAINT [FK_DocumentsProcessed_Sources] FOREIGN KEY ([SourceId]) REFERENCES [docid].[Sources] ([SourceId])
);




