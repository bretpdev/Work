CREATE TABLE [dbo].[ProjectFiles] (
    [ProjectFileId]   INT            IDENTITY (1, 1) NOT NULL,
    [ProjectId]       INT            NOT NULL,
    [SourceRoot]      NVARCHAR (256) NOT NULL,
	[SourcePathTypeId] int not null default 0,
    [DestinationRoot] NVARCHAR (256) NOT NULL,
	[DestinationPathTypeId] int not null default 0,
    [SearchPattern]   NVARCHAR (128) NULL,
	[AntiSearchPattern] NVARCHAR(128) null,
    [DecryptFile]   BIT            NOT NULL,
    [CompressFile]   BIT            NOT NULL,
    [EncryptFile] BIT,
	[AggregationFormatString] NVARCHAR(64) null ,
	[RenameFormatString] NVARCHAR(64) null ,
    [FixLineEndings] BIT NOT NULL DEFAULT 0, 
	[Retired] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_ProjectFiles] PRIMARY KEY CLUSTERED ([ProjectFileId] ASC),
    CONSTRAINT [FK_ProjectFiles_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([ProjectId]), 
    CONSTRAINT [FK_ProjectFiles_PathTypesSource] FOREIGN KEY ([SourcePathTypeId]) REFERENCES [PathTypes]([PathTypeId]),
	CONSTRAINT [FK_ProjectFiles_PathTypesDestination] FOREIGN KEY ([DestinationPathTypeId]) REFERENCES [PathTypes]([PathTypeId]), 
    CONSTRAINT [CK_ProjectFiles_CompressAggregate] CHECK ([AggregationFormatString] is null or CompressFile = 1), --can only aggregate zip files
    CONSTRAINT [CK_ProjectFiles_FormatStrings] CHECK (AggregationFormatString is null or RenameFormatString is null), --can't have a rename and an aggregation
    CONSTRAINT [CK_ProjectFiles_Patterns] CHECK (SearchPattern is not null or AntiSearchPattern is not null) --if both are null, no results will ever be pulled
); 


GO