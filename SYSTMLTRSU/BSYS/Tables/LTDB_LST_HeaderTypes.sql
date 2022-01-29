CREATE TABLE [dbo].[LTDB_LST_HeaderTypes] (
    [HeaderTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [HeaderType]   VARCHAR (50) NOT NULL,
    [CreatedBy]    VARCHAR (50) NOT NULL,
    [CreatedOn]    DATETIME     DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]    VARCHAR (50) NULL,
    [UpdatedAt]    DATETIME     NULL,
    [Active]       BIT          DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([HeaderTypeId] ASC)
);
