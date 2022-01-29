CREATE TABLE [complaints].[ComplaintHistory]
(
	[ComplaintHistoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[ComplaintId] int NOT NULL,
    [AddedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(),
    [HistoryDetail] NVARCHAR(4000) NOT NULL, 
    CONSTRAINT [FK_ComplaintHistory_Complaints] FOREIGN KEY ([ComplaintId]) REFERENCES [complaints].[Complaints]([ComplaintId])
)
