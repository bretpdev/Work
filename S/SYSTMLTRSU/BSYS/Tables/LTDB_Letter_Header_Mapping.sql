CREATE TABLE [dbo].[LTDB_Letter_Header_Mapping] (
    [LetterHeaderMappingId] INT          IDENTITY (1, 1) NOT NULL,
    [LetterId]              INT          NOT NULL,
    [HeaderTypeId]          INT          NOT NULL,
    [HeaderId]              INT          NOT NULL,
    [Order]                 INT          NOT NULL,
    [CreatedBy]             VARCHAR (50) NOT NULL,
    [CreatedOn]             DATETIME     DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]             VARCHAR (50) NULL,
    [UpdatedAt]             DATETIME     NULL,
    [Active]                BIT          DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([LetterHeaderMappingId] ASC),
    CONSTRAINT [FK_HEADER] FOREIGN KEY ([HeaderId]) REFERENCES [dbo].[LTDB_File_Headers] ([HeaderId]),
    CONSTRAINT [FK_HEADER_TYPE] FOREIGN KEY ([HeaderTypeId]) REFERENCES [dbo].[LTDB_LST_HeaderTypes] ([HeaderTypeId])
);