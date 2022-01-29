CREATE TABLE [dbo].[SCKR_DAT_Testing] (
    [Request]            INT           NOT NULL,
    [Class]              NVARCHAR (3)  NOT NULL,
    [TestNo]             INT           NOT NULL,
    [TestType]           NVARCHAR (50) NOT NULL,
    [Tester]             NVARCHAR (50) NULL,
    [Unit]               NVARCHAR (50) NOT NULL,
    [Comments]           NTEXT         NULL,
    [ReturnReason]       NVARCHAR (50) NULL,
    [ReturnDescription]  NTEXT         NULL,
    [Status]             NVARCHAR (50) NULL,
    [StatusDate]         SMALLDATETIME NULL,
    [Update]             NTEXT         NULL,
    [PreviousTestStatus] NVARCHAR (50) NULL,
    CONSTRAINT [PK_SCKR_DAT_Testing] PRIMARY KEY CLUSTERED ([Request] ASC, [Class] ASC, [TestNo] ASC, [TestType] ASC, [Unit] ASC)
);

