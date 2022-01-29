CREATE TABLE [dbo].[SCKR_REF_TestUnits] (
    [TestSeqNo] INT           NOT NULL,
    [Unit]      NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SCKR_REF_TestUnits] PRIMARY KEY CLUSTERED ([Unit] ASC, [TestSeqNo] ASC)
);

