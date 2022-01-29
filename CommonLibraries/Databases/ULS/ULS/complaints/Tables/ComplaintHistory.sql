CREATE TABLE [complaints].[ComplaintHistory] (
    [ComplaintHistoryId] INT             IDENTITY (1, 1) NOT NULL,
    [ComplaintId]        INT             NOT NULL,
    [AddedOn]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [AddedBy]            NVARCHAR (50)   DEFAULT (suser_sname()) NOT NULL,
    [HistoryDetail]      NVARCHAR (4000) NOT NULL,
    PRIMARY KEY CLUSTERED ([ComplaintHistoryId] ASC),
    CONSTRAINT [FK_ComplaintHistory_Complaints] FOREIGN KEY ([ComplaintId]) REFERENCES [complaints].[Complaints] ([ComplaintId])
);

