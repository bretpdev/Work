CREATE TABLE [print].[ArcLoanHeaderMapping] (
    [ArcLoanHeaderMappingId] INT IDENTITY (1, 1) NOT NULL,
    [ArcScriptDataMappingId] INT NOT NULL,
    [HeaderNameId]           INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ArcLoanHeaderMappingId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_ArcLoanHeaderMapping_ToArcScriptDataMapping] FOREIGN KEY ([ArcScriptDataMappingId]) REFERENCES [print].[ArcScriptDataMapping] ([ArcScriptDataMappingId]),
    CONSTRAINT [FK_ArcLoanHeaderMapping_ToHeaderNames] FOREIGN KEY ([HeaderNameId]) REFERENCES [print].[HeaderNames] ([HeaderNameId])
);

