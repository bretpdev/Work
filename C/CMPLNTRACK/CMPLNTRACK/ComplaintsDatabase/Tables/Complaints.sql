CREATE TABLE [complaints].[Complaints]
(
	[ComplaintId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[AccountNumber] CHAR(10) NOT NULL,
	[BorrowerName] nvarchar(50) NOT NULL,
    [ComplaintTypeId] INT NOT NULL, 
	[ComplaintPartyId] INT NOT NULL,
	[ComplaintGroupId] INT NOT NULL,
	[ComplaintDate] datetime NOT NULL DEFAULT GETDATE(),
	[ControlMailNumber] nvarchar(100),
	[DaysToRespond] int,
	[NeedHelpTicketNumber] nvarchar(8),
    [ResolutionComplaintHistoryId] INT NULL, 
    [ComplaintDescription] nvarchar(4000) NOT NULL, 
    [AddedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    CONSTRAINT [FK_Complaints_ComplaintTypes] FOREIGN KEY ([ComplaintTypeId]) REFERENCES [complaints].[ComplaintTypes]([ComplaintTypeId]), 
    CONSTRAINT [FK_Complaints_ComplaintHistory] FOREIGN KEY ([ResolutionComplaintHistoryId]) REFERENCES [complaints].[ComplaintHistory]([ComplaintHistoryId]), 
    CONSTRAINT [FK_Complaints_ComplaintParties] FOREIGN KEY ([ComplaintPartyId]) REFERENCES [complaints].[ComplaintParties]([ComplaintPartyId]), 
    CONSTRAINT [FK_Complaints_ComplaintGroups] FOREIGN KEY ([ComplaintGroupId]) REFERENCES [complaints].[ComplaintGroups]([ComplaintGroupId])
)
