CREATE TABLE [dbo].[SCKR_REF_StatusPIR] (
    [Sequence] INT           IDENTITY (1, 1) NOT NULL,
    [Request]  INT           NOT NULL,
    [Class]    NVARCHAR (3)  NULL,
    [Status]   NVARCHAR (50) NULL,
    [Begin]    SMALLDATETIME NULL,
    [End]      SMALLDATETIME NULL,
    [Court]    NVARCHAR (50) NULL,
    CONSTRAINT [PK_refStatusPIR] PRIMARY KEY CLUSTERED ([Sequence] ASC) WITH (FILLFACTOR = 90)
);

