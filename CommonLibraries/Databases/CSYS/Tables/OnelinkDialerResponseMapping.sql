CREATE TABLE [dbo].[OnelinkDialerResponseMapping]
(
	[OnelinkDialerResponseMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DispositionCode] VARCHAR(2) NOT NULL, 
    [ActionCode] VARCHAR(5) NOT NULL, 
    [ActivityType] VARCHAR(2) NOT NULL, 
    [ActivityContactType] VARCHAR(2) NOT NULL, 
    [Comment] VARCHAR(589) NOT NULL
)

GO
