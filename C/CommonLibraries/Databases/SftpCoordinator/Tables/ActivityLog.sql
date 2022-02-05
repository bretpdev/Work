CREATE TABLE [dbo].[ActivityLog] (
    [ActivityLogId]           BIGINT         IDENTITY (1, 1) NOT NULL,
	[CreatedOn]      datetime       NOT NULL DEFAULT getdate(),
    [ProjectFileId]   INT            NOT NULL,
	[RunHistoryId]    INT            NOT NULL,
    [SourcePath]      NVARCHAR (512) NOT NULL,
    [DestinationPath] NVARCHAR (512) NOT NULL,
	[DecryptionSuccessful] bit		null,
	[CopySuccessful] bit NOT null,
	[FixLineEndingsSuccessful] bit NULL,
	[DeleteSuccessful] bit null,
	[EncryptionSuccessful] bit null,
	[CompressionSuccessful] bit null,
    [Successful] BIT NOT NULL, 
    [InvalidFileId] INT NULL, 
    [PreDecryptionArchiveLocation] NVARCHAR(512) NULL, 
    [PreEncryptionArchiveLocation] NVARCHAR(512) NULL, 
    CONSTRAINT [PK_ActivityLog] PRIMARY KEY CLUSTERED ([ActivityLogId] ASC),
    CONSTRAINT [FK_ActivityLog_ProjectFiles] FOREIGN KEY ([ProjectFileId]) REFERENCES [dbo].[ProjectFiles] ([ProjectFileId]), 
    CONSTRAINT [FK_ActivityLog_RunHistory] FOREIGN KEY ([RunHistoryId]) REFERENCES [RunHistory]([RunHistoryId]), 
    CONSTRAINT [FK_ActivityLog_InvalidFiles] FOREIGN KEY ([InvalidFileId]) REFERENCES [InvalidFiles]([InvalidFileId])
);

