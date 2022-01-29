CREATE TABLE [dbo].[CentralizedPrintingFax] (
    [SeqNum]         INT          IDENTITY (1, 1) NOT NULL,
    [FaxNumber]      VARCHAR (50) NOT NULL,
    [AccountNumber]  VARCHAR (12) NOT NULL,
    [BusinessUnitId] INT          NOT NULL,
    [LetterId]       VARCHAR (10) NOT NULL,
    [Requested]      DATETIME     NOT NULL,
    [RightFaxHandle] VARCHAR (50) NULL,
    [Faxed]          DATETIME     NULL,
    [Confirmed]      DATETIME     NULL,
    [FinalStatus]    VARCHAR (20) NULL,
    CONSTRAINT [PK_CentralizedPrintingFax] PRIMARY KEY CLUSTERED ([SeqNum] ASC)
);

