CREATE TABLE [print].[ArcScriptDataMapping] (
    [ArcScriptDataMappingId] INT IDENTITY (1, 1) NOT NULL,
    [ScriptDataId]           INT NOT NULL,
    [ArcId]                  INT NOT NULL,
    [ArcTypeId]              INT DEFAULT ((1)) NOT NULL,
    [CommentId]              INT NULL,
    PRIMARY KEY CLUSTERED ([ArcScriptDataMappingId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_ArcScriptDataMapping_ToArcType] FOREIGN KEY ([ArcTypeId]) REFERENCES [dbo].[ArcType] ([ArcTypeId]),
    CONSTRAINT [FK_ArcScriptDataMapping_ToComments] FOREIGN KEY ([CommentId]) REFERENCES [print].[Comments] ([CommentId]),
    CONSTRAINT [FK_ArcScriptFileMapping_ToArcs] FOREIGN KEY ([ArcId]) REFERENCES [print].[Arcs] ([ArcId]),
    CONSTRAINT [FK_ArcScriptFileMapping_ToScriptData] FOREIGN KEY ([ScriptDataId]) REFERENCES [print].[ScriptData] ([ScriptDataId])
);

