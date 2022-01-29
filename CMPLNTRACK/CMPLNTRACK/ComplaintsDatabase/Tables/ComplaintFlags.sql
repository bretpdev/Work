CREATE TABLE [complaints].[ComplaintFlags]
(
	[ComplaintFlagId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ComplaintId] INT NOT NULL, 
    [FlagId] INT NOT NULL, 
    CONSTRAINT [FK_ComplaintFlags_Complaints] FOREIGN KEY (ComplaintId) REFERENCES [complaints].[Complaints]([ComplaintId]), 
    CONSTRAINT [FK_ComplaintFlags_Flags] FOREIGN KEY ([FlagId]) REFERENCES [complaints].[Flags]([FlagId])
)
