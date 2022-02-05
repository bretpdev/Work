CREATE TABLE [rcdialer].[SprocMapping]
(
	[SprocMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DaysOfWeekId] INT NOT NULL, 
    [SprocsId] INT NOT NULL, 
    CONSTRAINT [FK_SprocMapping_DayOFWeek] FOREIGN KEY ([DaysOfWeekId]) REFERENCES [rcdialer].[DaysOfWeek]([DaysOfWeekId]), 
    CONSTRAINT [FK_SprocMapping_Sprocs] FOREIGN KEY ([SprocsId]) REFERENCES [rcdialer].[Sprocs]([SprocsId])
)
