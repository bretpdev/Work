CREATE TABLE [calls].[Reasons]
(
	[ReasonId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[CategoryId] INT NOT NULL,
    [ReasonText] NVARCHAR(200) NOT NULL, 
    [Uheaa] BIT NOT NULL, 
    [Cornerstone] BIT NOT NULL, 
    [Inbound] BIT NOT NULL, 
    [Outbound] BIT NOT NULL, 
    [Enabled] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_Reasons_Categories] FOREIGN KEY (CategoryId) REFERENCES [calls].[Categories](CategoryId),
	CONSTRAINT [UK_Reasons_ReasonText] UNIQUE (ReasonText, CategoryId)
)
