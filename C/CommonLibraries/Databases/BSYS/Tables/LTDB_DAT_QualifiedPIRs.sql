CREATE TABLE [dbo].[LTDB_DAT_QualifiedPIRs] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Request]  INT            NULL,
    [Title]    NVARCHAR (100) NULL,
    [DocName]  NVARCHAR (50)  NULL,
    [Reviewer] NVARCHAR (50)  NULL,
    [Begin]    SMALLDATETIME  NULL,
    [End]      SMALLDATETIME  NULL,
    CONSTRAINT [PK_LTDB_DAT_QualifiedPIRs] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

