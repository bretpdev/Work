CREATE TABLE [dbo].[LTDB_REF_EMail] (
    [Sequence]  INT           IDENTITY (1, 1) NOT NULL,
    [Request]   INT           NULL,
    [Recipient] NVARCHAR (50) NULL,
    CONSTRAINT [PK_LTDB_REF_EMail] PRIMARY KEY CLUSTERED ([Sequence] ASC) WITH (FILLFACTOR = 90)
);

