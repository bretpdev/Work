CREATE TABLE [dbo].[BORR_Delinquency] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [LD_DLQ_OCC]    VARCHAR (10) NULL,
    [cur_dlq]       INT          NULL,
    CONSTRAINT [PK_BORR_Delinquency] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);

