CREATE TABLE [dbo].[LTDB_File_Headers] (
    [HeaderId]  INT          IDENTITY (1, 1) NOT NULL,
    [Header]    VARCHAR (50) NOT NULL,
    [CreatedBy] VARCHAR (50) NOT NULL,
    [CreatedOn] DATETIME     DEFAULT (getdate()) NOT NULL,
    [UpdatedBy] VARCHAR (50) NULL,
    [UpdatedAt] DATETIME     NULL,
    [Active]    BIT          DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([HeaderId] ASC),
    UNIQUE NONCLUSTERED ([Header] ASC)
);