CREATE TABLE [dbo].[QSTA_LST_RefreshQueue] (
    [Campaign]     NVARCHAR (10) NOT NULL,
    [Department]   NVARCHAR (3)  NOT NULL,
    [RefreshQueue] NVARCHAR (5)  NOT NULL,
    CONSTRAINT [PK_QS_RefreshQueue] PRIMARY KEY CLUSTERED ([Campaign] ASC)
);

