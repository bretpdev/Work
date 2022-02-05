CREATE TABLE [dbo].[SCKR_REF_PIRInitials] (
    [Request]       INT           NOT NULL,
    [Class]         NVARCHAR (50) NOT NULL,
    [Unit]          NVARCHAR (50) NOT NULL,
    [ReviewNo]      INT           NOT NULL,
    [InitialedDate] SMALLDATETIME NULL,
    [Initialed]     BIT           CONSTRAINT [DF_SCKR_REF_PIRInitials_Initialed] DEFAULT (0) NOT NULL,
    [Initialer]     NVARCHAR (50) NULL,
    CONSTRAINT [PK_refPIRInitials] PRIMARY KEY CLUSTERED ([Request] ASC, [Class] ASC, [Unit] ASC, [ReviewNo] ASC) WITH (FILLFACTOR = 90)
);

