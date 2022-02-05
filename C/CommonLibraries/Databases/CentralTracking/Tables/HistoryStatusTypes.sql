CREATE TABLE [dbo].[HistoryStatusTypes] (
    [HistoryStatusTypeId] INT          NOT NULL IDENTITY,
    [HistoryStatusTypeDescription]         VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_HistoryStatusTypes] PRIMARY KEY CLUSTERED ([HistoryStatusTypeId] ASC), 
    CONSTRAINT [AK_HistoryStatusTypes_Column] UNIQUE ([HistoryStatusTypeDescription])
);

