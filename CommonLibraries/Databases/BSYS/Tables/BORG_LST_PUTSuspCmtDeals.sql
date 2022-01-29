CREATE TABLE [dbo].[BORG_LST_PUTSuspCmtDeals] (
    [Description] VARCHAR (50) NOT NULL,
    [DealNumber]  VARCHAR (50) NOT NULL,
    [Owner]       VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_BORG_LST_PUTSuspCmtDeals] PRIMARY KEY CLUSTERED ([Description] ASC)
);

