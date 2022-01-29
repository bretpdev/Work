CREATE TABLE [dbo].[LTDB_REF_Status] (
    [Sequence] INT           IDENTITY (1, 1) NOT NULL,
    [Request]  INT           NOT NULL,
    [Status]   NVARCHAR (50) NULL,
    [Begin]    SMALLDATETIME NULL,
    [End]      SMALLDATETIME NULL,
    [Court]    NVARCHAR (50) NULL,
    CONSTRAINT [PK_LTDB_REF_Status] PRIMARY KEY CLUSTERED ([Sequence] ASC) WITH (FILLFACTOR = 90)
);

