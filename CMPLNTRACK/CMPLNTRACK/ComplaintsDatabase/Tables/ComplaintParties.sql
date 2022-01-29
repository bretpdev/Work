CREATE TABLE [complaints].[ComplaintParties]
(
	[ComplaintPartyId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PartyName] NVARCHAR(100) NOT NULL, 
    [AddedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] NVARCHAR(50) NULL
)
