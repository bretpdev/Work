CREATE TABLE [dbo].[LocateTypes] (
    [LocateType]       NCHAR (3)     NOT NULL,
    [ShortDescription] VARCHAR (50)  NOT NULL,
    [LongDescription]  VARCHAR (MAX) NOT NULL,
    [Ord]              INT           NOT NULL,
    CONSTRAINT [PK_LocateTypes] PRIMARY KEY CLUSTERED ([LocateType] ASC)
);

