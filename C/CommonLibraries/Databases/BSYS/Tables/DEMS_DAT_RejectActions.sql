CREATE TABLE [dbo].[DEMS_DAT_RejectActions] (
    [DemographicsSource] VARCHAR (50)  NOT NULL,
    [RejectReason]       VARCHAR (100) NOT NULL,
    [ActionCodeAddress]  VARCHAR (5)   NULL,
    [ActionCodePhone]    VARCHAR (5)   NULL,
    [ActionCodeEmail]    VARCHAR (5)   NULL,
    CONSTRAINT [PK_DEMS_DAT_RejectActions] PRIMARY KEY CLUSTERED ([DemographicsSource] ASC, [RejectReason] ASC)
);

