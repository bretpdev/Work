CREATE TABLE [dbo].[GENR_LST_LocateTypes] (
    [LocateType]       CHAR (3)      NOT NULL,
    [ShortDescription] VARCHAR (100) NULL,
    [LongDescription]  VARCHAR (200) NULL,
    CONSTRAINT [PK_GENR_LST_LocateTypes] PRIMARY KEY CLUSTERED ([LocateType] ASC)
);

