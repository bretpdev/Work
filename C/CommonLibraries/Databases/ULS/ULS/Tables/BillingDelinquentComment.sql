CREATE TABLE [dbo].[BillingDelinquentComment]
(
	[BillingDelinquentCommentId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StartCount] INT NOT NULL, 
    [EndCount] INT NOT NULL, 
    [Comment] VARCHAR(500) NULL
)
