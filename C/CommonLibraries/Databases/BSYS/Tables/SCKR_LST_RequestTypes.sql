CREATE TABLE [dbo].[SCKR_LST_RequestTypes] (
    [RequestType] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_lstRequestTypes] PRIMARY KEY CLUSTERED ([RequestType] ASC) WITH (FILLFACTOR = 90)
);

