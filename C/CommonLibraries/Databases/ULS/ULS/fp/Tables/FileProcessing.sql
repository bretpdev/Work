CREATE TABLE [fp].[FileProcessing]
(
	[FileProcessingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [GroupKey] VARCHAR(50) NULL, 
    [ScriptFileId] INT NULL, 
    [SourceFile] VARCHAR(100) NOT NULL, 
    [ProcessedAt] DATETIME NULL, 
    [CreatedBy] VARCHAR(50) NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT getdate(), 
    [Active] BIT NOT NULL DEFAULT 1, 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL, 
    CONSTRAINT [FK_FileProcessing_ScriptFiles] FOREIGN KEY ([ScriptFileId]) REFERENCES fp.[ScriptFiles]([ScriptFileId]) 
)