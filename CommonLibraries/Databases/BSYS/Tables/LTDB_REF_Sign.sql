CREATE TABLE [dbo].[LTDB_REF_Sign] (
    [Request]    INT           NOT NULL,
    [Unit]       NVARCHAR (50) NOT NULL,
    [Class]      INT           NULL,
    [Signed]     BIT           CONSTRAINT [DF_LTDB_REF_Sign_Signed] DEFAULT (0) NOT NULL,
    [Agent]      NVARCHAR (50) NULL,
    [SignedDate] SMALLDATETIME NULL,
    CONSTRAINT [PK_LTDB_REF_Sign] PRIMARY KEY CLUSTERED ([Request] ASC, [Unit] ASC) WITH (FILLFACTOR = 90)
);

