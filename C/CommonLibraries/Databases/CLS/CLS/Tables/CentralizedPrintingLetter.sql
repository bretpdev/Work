CREATE TABLE [dbo].[CentralizedPrintingLetter] (
    [SeqNum]                        INT          IDENTITY (1, 1) NOT NULL,
    [LetterId]                      VARCHAR (10) NOT NULL,
    [AccountNumber]                 VARCHAR (12) NOT NULL,
    [BusinessUnitId]                INT          NOT NULL,
    [IsDomestic]                    BIT          CONSTRAINT [DF_CentralizedPrintingLetter_IsDomestic] DEFAULT ((0)) NOT NULL,
    [Requested]                     DATETIME     NOT NULL,
    [StateMailBatchSeq]             INT          NULL,
    [PickedUpByCentralizedPrinting] DATETIME     NULL,
    [Printed]                       DATETIME     NULL,
    CONSTRAINT [PK_CentralizedPrintingLetter_1] PRIMARY KEY CLUSTERED ([SeqNum] ASC)
);

