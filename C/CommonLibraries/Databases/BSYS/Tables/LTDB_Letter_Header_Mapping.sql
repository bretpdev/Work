CREATE TABLE [dbo].[LTDB_Letter_Header_Mapping]
(
	[LetterHeaderMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LetterId] INT NOT NULL
		CONSTRAINT FK_LETTER_ID
			REFERENCES  LTDB_DAT_DocDetail(DocDetailId), 
    [HeaderTypeId] INT NOT NULL
		CONSTRAINT FK_HEADER_TYPE
			REFERENCES  LTDB_LST_HeaderTypes(HeaderTypeId),
    [HeaderId] INT NOT NULL
		CONSTRAINT FK_HEADER
			REFERENCES  LTDB_File_Headers(HeaderId),
    [Order] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL, 
    [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [UpdatedBy] VARCHAR(50) NULL, 
    [UpdatedAt] DATETIME NULL, 
    [Active] BIT NULL DEFAULT 1
)
